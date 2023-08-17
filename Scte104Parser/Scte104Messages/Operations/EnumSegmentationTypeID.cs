/* =============================================
 * Copyright 2021 TVU Networks Co.,Ltd. All rights reserved.
 * For internal members in TVU Networks only.
 * FileName: EnumSegmentationTypeID.cs
 * Purpose:
 * Author:   YulongLu added on 12.7th, 2021.
 * Since:    Microsoft Visual Studio 2019
 * =============================================*/

namespace Scte104Parser.Scte104Messages.Operations
{
    public enum EnumSegmentationTypeID
    {
        NotIndicated = 0x00,
        ContentIdentification = 0x01,
        ProgramStart = 0x10,
        ProgramEnd = 0x11,
        ProgramEarlyTermination = 0x12,
        ProgramBreakaway = 0x13,
        ProgramResumption = 0x14,
        ProgramRunoverPlanned = 0x15,
        ProgramRunoverUnplanned = 0x16,
        ProgramOverlapStart = 0x17,
        ProgramBlackoutOverride = 0x18,
        ProgramStartInProgress = 0x19,
        ChapterStart = 0x20,
        ChapterEnd = 0x21,
        BreakStart = 0x22,
        BreakEnd = 0x23,
        OpeningCreditStart = 0x24,
        OpeningCreditEnd = 0x25,
        ClosingCreditStart = 0x26,
        ClosingCreditEnd = 0x27,
        ProviderAdvertisementStart = 0x30,
        ProviderAdvertisementEnd = 0x31,
        DistributorAdvertisementStart = 0x32,
        DistributorAdvertisementEnd = 0x33,
        ProviderPlacementOpportunityStart = 0x34,
        ProviderPlacementOpportunityEnd = 0x35,
        DistributorPlacementOpportunityStart = 0x36,
        DistributorPlacementOpportunityEnd = 0x37,
        ProviderOverlayPlacementOpportunityStart = 0x38,
        ProviderOverlayPlacementOpportunityEnd = 0x39,
        DistributorOverlayPlacementOpportunityStart = 0x3A,
        DistributorOverlayPlacementOpportunityEnd = 0x3B,
        ProviderPromoStart = 0x3C,
        ProviderPromoEnd = 0x3D,
        DistributorPromoStart = 0x3E,
        DistributorPromoEnd = 0x3F,
        UnscheduledEventStart = 0x40,
        UnscheduledEventEnd = 0x41,
        AlternateContentOpportunityStart = 0x42,
        AlternateContentOpportunityEnd = 0x43,
        ProviderAdBlockStart = 0x44,
        ProviderAdBlockEnd = 0x45,
        DistributorAdBlockStart = 0x46,
        DistributorAdBlockEnd = 0x47,
        NetworkEventStart = 0x50,
        NetworkEventEnd = 0x51,
    }
}