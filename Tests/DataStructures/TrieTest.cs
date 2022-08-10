using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using DataStructuresDotNet.Trees;
using Xunit;
using System;

namespace Tests.DataStructures
{
    public class TrieTest
    {
        private Trie trie;
        private TrieMap<int> trieMap;

        static string prefix_howTo = "How to make";
        static string prefix_act = "act";

        string word_howToSand = prefix_howTo + "a sandwitch";
        string word_howToRobot = prefix_howTo +  "a robot";
        string word_howToOmelet = prefix_howTo + "an omlet";
        string word_howToProp = prefix_howTo + "a proposal";

        string word_acts = prefix_act + "s";
        string word_actor = prefix_act + "or";
        string word_acting = prefix_act + "ing";
        string word_actress = prefix_act + "ress";
        string word_active = prefix_act + "ive";


        List<string> listHowTo;

        List<string> listActors;

        public TrieTest()
        {
            trie = new Trie();
            /*trieMap = new TrieMap<string>();*/
            trieMap = new TrieMap<int>();


            trie.Add(word_howToOmelet);
            trie.Add(word_howToSand);
            trie.Add(word_howToRobot);
            trie.Add(word_howToProp);

            listHowTo = new List<string>()
            {
                word_howToSand,
                word_howToRobot,
                word_howToOmelet,
                word_howToProp,
            };

            trieMap.Add(word_actress, 82);
            trieMap.Add(word_active, 65);
            trieMap.Add(word_acting, 34);
            trieMap.Add(word_acts, 81);
            trieMap.Add(word_actor, 32);
            
            listActors = new List<string>()
            {
                word_actress,
                word_active,
                word_acting,
                word_acts,
                word_actor,
            };
        }

        [Fact]
        public void TrieMap_Add_CheckoutCorrectConstructorInput()
        {
            Assert.Equal(listActors.Count, trieMap.Count);
        }

        [Fact]
        public void Add_CheckCorrectContructorInputTrie()
        {
            Assert.Equal(4, trie.Count);
        }

        [Fact]
        public void TrieMap_Add_TryAddEmptyString()
        {
            Assert.Throws<ArgumentNullException>(() => trieMap.Add("", 12));
        }

        /// <summary>
        /// Let's try to add empty string
        /// </summary>
        [Fact]
        public void Add_TryToAddEmptyString()
        {
            Assert.Throws<ArgumentNullException>(() => trie.Add(""));
        }

        [Fact]
        public void TrieMap_Contains_SearchAddedElements()
        {
            Assert.True(trieMap.ContainsWord(word_acts));
            // try search another word
            Assert.False(trieMap.ContainsWord(word_howToOmelet));
            
            // search by prefix
            Assert.True(trieMap.ContainsPrefix(prefix_act));

            List<string> prefixActi = trieMap.SearchByPrefix("acti").Select(item => item.Key).ToList();
            Assert.Contains(word_active, prefixActi);
            
            // try to emtpy string
            Assert.Throws<InvalidOperationException>(() => trieMap.ContainsWord(""));
        }

        [Fact]
        public void Contains_SearchAddedElements()
        {
            Assert.True(trie.ContainsWord(word_howToOmelet));
            Assert.False(trie.ContainsWord("Unexisting value"));
            // search by prefix
            Assert.True(trie.ContainsPrefix(prefix_howTo));
            // search by prefix emtpy string
            Assert.False(trie.ContainsPrefix("Some Prefix"));
            // search for empty string should throw error!
            Assert.Throws<InvalidOperationException>(() => trie.ContainsWord(""));
        }

        [Fact]
        public void TrieMap_Search_SearchAndReturnValuesToCheck()
        {
            List<String> values = trieMap.SearchByPrefix(prefix_act).Select<KeyValuePair<String, int>, String>(item => item.Key).ToList();

            Assert.Equal(listActors.Count, values.Count);

            foreach (var value in values)
            {
                Assert.Contains(value, listActors);
            }
        }

        [Fact]
        public void Search_SearchAndReturnValuesToCheck()
        {
            List<string> values = trie.SearchByPrefix(prefix_howTo).ToList();

            Assert.Equal(listHowTo.Count, values.Count);

            foreach (var value in values)
            {
                Assert.Contains(value, listHowTo);
            }
        }

        [Fact]
        public void MapTrie_Remove_RemoveWord()
        {
            // let's try to remove non terminal word
            Assert.Throws<KeyNotFoundException>(() => trieMap.Remove("acto"));
            Assert.Equal(listActors.Count, trieMap.Count);

            // let's try to remvoe terminal word
            trieMap.Remove(word_acting);
            Assert.False(trieMap.ContainsWord(word_acting));
            Assert.Equal(listActors.Count - 1, trieMap.Count);
        }

        [Fact]
        public void Remove_RemoveWordFromTrie()
        {
            // let's try to remove non terminal word
            Assert.Throws<KeyNotFoundException>(() => trie.Remove("How to make banana"));
            // and check count
            Assert.Equal(listHowTo.Count, trie.Count);

            trie.Remove(word_howToSand);
            // removed 1 item
            Assert.False(trie.ContainsWord(word_howToSand));
            Assert.Equal(listHowTo.Count - 1, trie.Count);
        }

        [Fact]
        public void MapTrie_Enumerator_CheckAllWordsReturnedByEnumerator()
        {
            var enumerator = trieMap.GetEnumerator();
            var allWords = new List<string>();

            while (enumerator.MoveNext())
            {
                allWords.Add(enumerator.Current.Key);
            }

            Assert.Equal(listActors.Count, allWords.Count);

            foreach (var value in allWords)
            {
                Assert.Contains(value, allWords);
            }
        }

        [Fact]
        public void Enumerator_CheckAllWordsReturnedByEnumerator()
        {
            var enumerator = trie.GetEnumerator();
            var allWords = new List<string>();
            while (enumerator.MoveNext())
            {
                allWords.Add(enumerator.Current);
            }

            Assert.Equal(trie.Count, allWords.Count);
            // Assert each element
            foreach (var word in allWords)
                Assert.Contains(word, listHowTo);
        }

        [Fact]
        public void TrieMap_Cear_CheckCountAfterClear()
        {
            trieMap.Clear();
            Assert.Equal(0, trieMap.Count);
            Assert.False(trieMap.ContainsWord(word_acts));
            Assert.False(trieMap.ContainsPrefix(prefix_act));
        }

        [Fact]
        public void Clear_CheckCountAfterClearTrie()
        {
            trie.Clear();
            Assert.Equal(0, trie.Count);
            Assert.False(trie.ContainsPrefix(prefix_howTo));
            Assert.False(trie.ContainsWord(word_howToSand));
        }
    }
}
