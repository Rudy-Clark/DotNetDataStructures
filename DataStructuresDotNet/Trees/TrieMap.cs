using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace DataStructuresDotNet.Trees
{
    /// <summary>
    /// The Trie Map Data Structure (a.k.a Prefix Tree).
    /// </summary>
    /// <typeparam name="TRecord">The type of records attached to words</typeparam>
    public class TrieMap<TRecord> : IDictionary<String, TRecord>, IEnumerable<KeyValuePair<String, TRecord>>
    {
        private TrieMapNode<TRecord> root { get; set; }
        private EqualityComparer<TRecord> recordComparer = EqualityComparer<TRecord>.Default;

        /// <summary>
        ///  CONSTRUCTOR
        /// </summary>
        public TrieMap()
        {
            Count = 0;
            root = new TrieMapNode<TRecord>(' ', default, false);
        }

        public int Count
        {
            get;
            private set;
        }

        public bool IsEmpty
        {
            get => Count == 0;
        }

        public void Add(string word, TRecord record)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentNullException("Word is empty or null.");

            TrieMapNode<TRecord> current = root;

            for (int i = 0; i < word.Length; ++i)
            {
                if (!current.Children.ContainsKey(word[i]))
                {
                    TrieMapNode<TRecord> newTrieMapNode = new(word[i], record);
                    newTrieMapNode.Parent = current;
                    current.Children.Add(word[i], newTrieMapNode);
                }
                current = current.Children[word[i]];
            }

            if (current.IsTerminal)
            {
                throw new InvalidOperationException("Word already exists in Trie.");
            }

            ++Count;
            current.IsTerminal = true;
            current.Record = record;
        }

        /// <summary>
        /// Updates a terminal word with a new record. Throws an exception if word was not found or if it is not a terminal word.
        /// </summary>
        public void UpdateWord(string word, TRecord newRecord)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentException("Word is empty or null.");

            TrieMapNode<TRecord> current = root;

            for (int i = 0; i < word.Length; ++i)
            {
                if (!current.Children.ContainsKey(word[i]))
                {
                    throw new KeyNotFoundException("Word doesn't belong to trie.");
                }

                current = current.Children[word[i]];
            }

            if (!current.IsTerminal)
            {
                throw new KeyNotFoundException("Word doesn't belong to trie.");
            }

            current.Record = newRecord;
        }

        /// <summary>
        /// Removes a word from the trie.
        /// </summary>
        public void Remove(string word)
        {
            if (string.IsNullOrEmpty(word))
                throw new ArgumentException("Word is empty or null.");

            TrieMapNode<TRecord> current = root;

            for (int i = 0; i < word.Length; ++i)
            {
                if (!current.Children.ContainsKey(word[i]))
                {
                    throw new KeyNotFoundException("Word doesn't belong to trie!");
                }

                current = current.Children[word[i]];
            }

            if (!current.IsTerminal)
            {
                throw new KeyNotFoundException("Word doesn't belong to trie!");
            }

            --Count;
            current.Remove();
        }

        /// <summary>
        /// Checks whether the trie has a specific word.
        /// </summary>
        public bool ContainsWord(string word)
        {
            TRecord record;
            return SearchByWord(word, out record);
        }

        /// <summary>
        /// Checks whether the trie has a specific prefix.
        /// </summary>
        public bool ContainsPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new InvalidOperationException("Prefix is either null or empty.");

            TrieMapNode<TRecord> current = root;

            for (int i = 0; i < prefix.Length; ++i)
            {
                if (!current.Children.ContainsKey(prefix[i]))
                {
                    return false;
                }

                current = current.Children[prefix[i]];

            }

            return true;
        }

        /// <summary>
        /// Searchs the trie for a word and returns the associated record, if found; otherwise returns false.
        /// </summary>
        public bool SearchByWord(string word, out TRecord record)
        {
            if (string.IsNullOrEmpty(word))
                throw new InvalidOperationException("Word is either null or empty.");

            record = default(TRecord);
            TrieMapNode<TRecord> current = root;

            for (int i = 0; i < word.Length; ++i)
            {
                if (!current.Children.ContainsKey(word[i]))
                {
                    return false;
                }

                current = current.Children[word[i]];

            }

            if (!current.IsTerminal)
            {
                return false;
            }

            record = current.Record;
            return true;
        }

        /// <summary>
        /// Searches the entire trie for words that has a specific prefix.
        /// </summary>
        public IEnumerable<KeyValuePair<String, TRecord>> SearchByPrefix(string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                throw new InvalidOperationException("Prefix is either null or empty.");

            TrieMapNode<TRecord> current = root;

            for (int i = 0; i < prefix.Length; ++i)
            {
                if (!current.Children.ContainsKey(prefix[i]))
                {
                    throw null;
                }

                current = current.Children[prefix[i]];

            }

            return current.GetByPrefix();
        }

        /// <summary>
        /// Clears this insance.
        /// </summary>
        public void Clear()
        {
            Count = 0;
            root.Clear();
            root = new TrieMapNode<TRecord>(' ', default, false);
        }

        #region IDictionary implementation
        public bool IsReadOnly
        {
            get => false;
        }

        public bool ContainsKey(string key)
        {
            TRecord record;
            return SearchByWord(key, out record);
        }

        /// <summary>
        /// Return all terminal words in trie.
        /// </summary>
        public ICollection<string> Keys
        {
            get
            {
                var collection = new List<string>();

                var terminalNodes = root.GetTerminalChildren();

                foreach (var node in terminalNodes)
                {
                    collection.Add(node.Word);
                }

                return collection;
            }
        }

        public ICollection<TRecord> Values
        {
            get
            {
                var collection = new List<TRecord>();

                var terminalNodes = root.GetTerminalChildren();

                foreach (var node in terminalNodes)
                {
                    collection.Add(node.Record);
                }

                return collection;
            }
        }

        /// <summary>
        /// Tries to get the associated record of a terminal word from trie. Returns false if key was not found.
        /// </summary>
        public bool TryGetValue(string word, out TRecord value)
        {
            return SearchByWord(word, out value);
        }

        public bool Contains(KeyValuePair<string, TRecord> item)
        {

            TRecord record;
            bool status = SearchByWord(item.Key, out record);
            return (status && recordComparer.Equals(item.Value, record));
        }

        public void CopyTo(KeyValuePair<String, TRecord>[] array, int arrayIndex)
        {
            var tempArray = root.GetTerminalChildren()
                .Select<TrieMapNode<TRecord>, KeyValuePair<String, TRecord>>(item => new KeyValuePair<String, TRecord>(item.Word, item.Record))
                .ToArray();
            Array.Copy(tempArray, 0, array, arrayIndex, Count);
        }

        /// <summary>
        /// Get/Set the associated record of a terminal word in trie.
        /// </summary>
        public TRecord this[string key]
        {
            get
            {
                TRecord record;
                if (SearchByWord(key, out record))
                {
                    return record;
                }
                throw new KeyNotFoundException();
            }
            set
            {
                UpdateWord(key, value);
            }
        }

        public void Add(KeyValuePair<String, TRecord> item)
        {
            Add(item.Key, item.Value);
        }

        bool IDictionary<String, TRecord>.Remove(string key)
        {
            try
            {
                Remove(word: key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Removes a word from trie.
        /// </summary>
        bool ICollection<KeyValuePair<string, TRecord>>.Remove(KeyValuePair<string, TRecord> item)
        {
            try
            {
                Remove(word: item.Key);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region IEnumerable implementation
        public IEnumerator<KeyValuePair<String, TRecord>> GetEnumerator()
        {
            return root.GetTerminalChildren().Select<TrieMapNode<TRecord>, KeyValuePair<String, TRecord>>(item => new KeyValuePair<String, TRecord>(item.Word, item.Record)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw (Exception)GetEnumerator();
        }

        #endregion
    }
}
