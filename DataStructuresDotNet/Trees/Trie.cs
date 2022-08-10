/***
 * Trie.
 * 
 * This is the standard/vanilla implementation of a Trie. For an associative version of Trie, checkout the TrieMap<TRecord> class.
 * 
 * This class implements the IEnumerable interface.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructuresDotNet.Trees
{
    /// <summary>
    /// The vanila Trie implementation.
    /// </summary>
    public class Trie : IEnumerable<String>
    {
        private TrieNode root { get; set; }

        public int Count
        {
            private set;
            get;
        }

        public Trie()
        {
            Count = 0;
            root = new TrieNode(' ', false);
        }

        /// <summary>
        /// Checks if element is empty.
        /// </summary>
        public bool IsEmpty
        {
            get => Count == 0;
        }

        public void Add(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentNullException("Word is empty or null!");
            }

            var current = root;

            for (int i = 0; i < word.Length; i++)
            {
                if (!current.Children.ContainsKey(word[i]))
                {
                    TrieNode trieNode = new(word[i]);
                    trieNode.Parent = current;
                    current.Children.Add(word[i], trieNode);
                }

                current = current.Children[word[i]];
            }

            if (current.IsTerminal)
            {
                throw new InvalidOperationException("Word already exists on trie!");
            }

            ++Count;
            current.IsTerminal = true;
        }

        /// <summary>
        /// Removes a word from the trie.
        /// </summary>
        public void Remove(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentNullException("Word is empty or null!");
            }

            var current = root;

            for (int i = 0; i < word.Length; ++i)
            {
                if (!current.Children.ContainsKey(word[i]))
                    throw new KeyNotFoundException("Word doesn't belong to trie.");
                current = current.Children[word[i]];
            }

            if (!current.IsTerminal)
            {
                throw new KeyNotFoundException("Word doesn't belong to trie.");
            }

            --Count;
            current.Remove();
        }

        /// <summary>
        /// Checks whether the trie has a specific word.
        /// </summary>
        public bool ContainsWord(string word)
        {
            if (string.IsNullOrEmpty(word))
                throw new InvalidOperationException("Word is either null or empty.");

            var current = root;

            for (int i = 0; i < word.Length; i++)
            {
                if (!current.Children.ContainsKey(word[i]))
                {
                    return false;
                }

                current = current.Children[word[i]];
            }

            return current.IsTerminal;
        }

        /// <summary>
        /// Checks whether the trie has a specific prefix.
        /// </summary>
        public bool ContainsPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new InvalidOperationException("Word is either null or empty.");

            var current = root;

            for (int i = 0; i < prefix.Length; ++i)
            {
                if (!current.Children.ContainsKey(prefix[i]))
                    return false;

                current = current.Children[prefix[i]];

            }

            return true;
        }

        /// <summary>
        /// Searches the entire trie for words that has a specific prefix.
        /// </summary>
        public IEnumerable<string> SearchByPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new InvalidOperationException("Word is either null or empty.");

            var current = root;

            for (int i = 0; i < prefix.Length; i++)
            {
                if (!current.Children.ContainsKey(prefix[i]))
                {
                    return null;
                }

                current = current.Children[prefix[i]];

            }

            return current.GetByPrefix();
        }

        // <summary>
        /// Clears this insance.
        /// </summary>
        public void Clear()
        {
            Count = 0;
            root.Clear();
            root = new TrieNode(' ', false);
        }

        #region IEnumerable<String> Implementation
        /// <summary>
        /// IEnumerable\<String\>.IEnumerator implementation.
        /// </summary>
        public IEnumerator<string> GetEnumerator()
        {
            return root.GetTerminalChildren().Select(node => node.Word).GetEnumerator();
        }

        /// <summary>
        /// IEnumerable\<String\>.IEnumerator implementation.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion IEnumerable<String> Implementation
    }
}
