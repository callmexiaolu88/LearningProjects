using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Runtime.CompilerServices;

namespace asyncEnumerable
{
    public class ConcreteAsyncEnumerable
    {
        public string Id{ get; }
        public ConcreteAsyncEnumerable()
        {
            Id = Guid.NewGuid().ToString();
        }

        public async IAsyncEnumerator<string> GetAsyncEnumerator()
        {
            await Task.Delay(1000);
            yield return $"Async {Id}:Hello";
            await Task.Delay(1000);
            yield return $"Async {Id}:World";
            await Task.Delay(1000);
            yield return $"Async {Id}:!";
            await Task.Delay(1000);
            yield return $"Async {Id}:this is end";
            await Task.Delay(1000);
            yield break;
        }

        public IEnumerator<Task<string>> GetEnumerator()
        {
            yield return Task.Delay(1000).ContinueWith<string>(t => $"{Id}:Hello");
            yield return Task.Delay(1000).ContinueWith<string>(t => $"{Id}:World");
            yield return Task.Delay(1000).ContinueWith<string>(t => $"{Id}:!");
            yield return Task.Delay(1000).ContinueWith<string>(t => $"{Id}:this is end");
            yield break;
        }

    }
}