/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: Scte104MultipleOperationMessage.cs
 * Purpose:
 * Author:   YulongLu added on 12.7th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using Scte104Parser.Scte104Messages.Operations;

namespace Scte104Parser.Scte104Messages
{
    internal class Scte104MultipleOperationMessage : Scte104Message
    {
        public override Scte104MessageType Type => Scte104MessageType.MultipleOperation;

        public ushort Reserved { get; set; }
        public ushort MessageSize { get; set; }
        public byte ProtocolVersion { get; set; }
        public byte AS_Index { get; set; }
        public byte MessageNumber { get; set; }
        public ushort DPI_PID_Index { get; set; }
        public byte SCTE35ProtocolVersion { get; set; }
        public Scte104Timestamp Timestamp { get; set; }
        public byte NumberOfOperation { get; set; }

        private List<Scte104MessageOperation> _operations = new List<Scte104MessageOperation>();
        public IEnumerable<Scte104MessageOperation> Operations => _operations;

        public Scte104MultipleOperationMessage()
        {
            Reserved = 0xFFFF;
            Timestamp = new Scte104Timestamp();
        }

        public override void Decode(Scte104Reader reader)
        {
            Reserved = reader.ReadUInt16();
            MessageSize = reader.ReadUInt16();
            ProtocolVersion = reader.ReadByte();
            AS_Index = reader.ReadByte();
            MessageNumber = reader.ReadByte();
            DPI_PID_Index = reader.ReadUInt16();
            SCTE35ProtocolVersion = reader.ReadByte();
            Timestamp = reader.ReadTimestamp();
            NumberOfOperation = reader.ReadByte();
            deocdeMessageOperations(reader);
        }

        public override void Encode(Scte104Writer writer)
        {
            var length = writer.Length;
            writer.WriteUInt16(Reserved);
            var range = writer.WriteUInt16(MessageSize);
            writer.WriteByte(ProtocolVersion);
            writer.WriteByte(AS_Index);
            writer.WriteByte(MessageNumber);
            writer.WriteUInt16(DPI_PID_Index);
            writer.WriteByte(SCTE35ProtocolVersion);
            writer.WriteTimestamp(Timestamp);
            writer.WriteByte((byte)_operations.Count);
            encodeMessageOperations(writer);
            writer.Replace(range.Start, (ushort)(writer.Length - length));
        }

        public override void Validate()
        {
            base.Validate();
            if (Reserved != 0xFF)
                throw new Exception($"Reserved [{Reserved}] is available.");
        }

        public void AddOperations(IEnumerable<Scte104MessageOperation> operations)
        {
            if (operations?.Any() == true)
            {
                _operations.AddRange(operations);
                NumberOfOperation = (byte)_operations.Count();
            }
        }

        public void SetOperations(IEnumerable<Scte104MessageOperation> operations)
        {
            _operations.Clear();
            if (operations?.Any() == true)
            {
                _operations.AddRange(operations);
            }
            NumberOfOperation = (byte)_operations.Count();
        }

        public override IEnumerable<Scte104OperationSummary> GetOperationSummaries()
            => Operations.Select(op => op.GetSummary());

        private void deocdeMessageOperations(Scte104Reader reader)
        {
            var exceptions = new List<Exception>();
            for (int i = 0; i < NumberOfOperation; i++)
            {
                var opID = reader.ReadUInt16();
                var dataLength = reader.ReadUInt16();
                var dataInfo = reader.ReadRawInfo(dataLength);
                Scte104MessageOperation op = operationFactory(opID);
                try
                {
                    op.Decode(dataInfo);
                    _operations.Add(op);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Any())
                throw new AggregateException("Decode message operation error.", exceptions);
        }

        private void encodeMessageOperations(Scte104Writer writer)
        {
            var exceptions = new List<Exception>();
            foreach (var op in _operations)
            {
                try
                {
                    writer.WriteUInt16((ushort)op.OperationID);
                    var range = writer.WriteUInt16(0);
                    var length = writer.Length;
                    op.Encode(writer);
                    writer.Replace(range.Start, (ushort)(writer.Length - length));
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            if (exceptions.Any())
                throw new AggregateException("Encode message operation error.", exceptions);
        }

        private Scte104MessageOperation operationFactory(int opID)
            => (EnumOperationID)opID switch
            {
                EnumOperationID.SpliceRequestData => new SpliceRequestOperation(),
                EnumOperationID.SpliceNull => new SpliceNullRequestOperation(),
                EnumOperationID.TimeSignalRequestData => new TimeSignalRequestOperation(),
                EnumOperationID.InsertDTMFDescriptor => new InsertDTMFDescriptorRquestOperation(),
                EnumOperationID.InsertSegmentationDescriptor => new InsertSegmentationDescriptorRequestOperation(),
                _ => new Scte104MessageDefaultOperation((EnumOperationID)opID),
            };
    }
}