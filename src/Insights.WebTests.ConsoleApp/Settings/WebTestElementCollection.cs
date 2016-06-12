using System.Configuration;
using System.Linq;

namespace Aliencube.Azure.Insights.WebTests.ConsoleApp.Settings
{
    /// <summary>
    /// Gets or sets the configuration element collection entity for web tests.
    /// </summary>
    public sealed class WebTestElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets the type of the <see cref="ConfigurationElementCollectionType"/> instance.
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.BasicMap;

        /// <summary>
        /// Gets the name used to identify this collection of elements in the configuration file
        /// when overridden in a derived class.
        /// </summary>
        protected override string ElementName => "webTest";

        /// <summary>
        /// Gets or sets the key/value pair element at the specified index location.
        /// </summary>
        /// <param name="index">The index location of the key/value pair element to remove.</param>
        /// <returns>Returns the key/value pair element at the specified index location.</returns>
        public WebTestElement this[int index]
        {
            get { return (WebTestElement)this.BaseGet(index); }
            set
            {
                if (this.BaseGet(index) != null)
                {
                    this.BaseRemoveAt(index);
                }

                this.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Gets or sets the key/value pair element having the specified key.
        /// </summary>
        /// <param name="key">Key value.</param>
        /// <returns>Returns the key/value pair element having the specified key.</returns>
        public new WebTestElement this[string key]
        {
            get { return (WebTestElement)this.BaseGet(key); }
            set
            {
                var item = (WebTestElement)this.BaseGet(key);
                if (item != null)
                {
                    var index = this.BaseIndexOf(item);
                    this.BaseRemoveAt(index);
                    this.BaseAdd(index, value);
                }

                this.BaseAdd(value);
            }
        }
        /// <summary>
        /// Creates a new instance of the <see cref="WebTestElement"/> class.
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new WebTestElement();
        }

        /// <summary>
        /// Gets the element key
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WebTestElement)element).TestType;
        }

        /// <summary>
        /// Adds an key/value pair element to the ConfigurationElementCollection.
        /// </summary>
        /// <param name="element">Item element.</param>
        public void Add(WebTestElement element)
        {
            this.BaseAdd(element);
        }

        /// <summary>
        /// Removes all key/value pair element objects from the collection.
        /// </summary>
        public void Clear()
        {
            this.BaseClear();
        }

        /// <summary>
        /// Removes an key/value pair element from the collection.
        /// </summary>
        /// <param name="key">Key value.</param>
        public void Remove(string key)
        {
            this.BaseRemove(key);
        }

        /// <summary>
        /// Removes the key/value pair element at the specified index location.
        /// </summary>
        /// <param name="index">The index location of the key/value pair element to remove.</param>
        public void RemoveAt(int index)
        {
            this.BaseRemoveAt(index);
        }

        /// <summary>
        /// Clones the current instance.
        /// </summary>
        /// <returns>Returns <see cref="WebTestElementCollection"/> instance cloned.</returns>
        public WebTestElementCollection Clone()
        {
            var collection = new WebTestElementCollection();
            foreach (var element in this.OfType<WebTestElement>())
            {
                collection.Add(element.Clone());
            }

            return collection;
        }
    }
}