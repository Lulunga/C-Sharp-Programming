using System;

namespace Inheritance.DataStructure
{
    public class Category : IComparable<Category>, IComparable
    {
        private string product;
        private MessageType type;
        private MessageTopic topic;

        public Category(string product, MessageType type, MessageTopic topic)
        {
            this.product = product;
            this.type = type;
            this.topic = topic;
        }

        public override bool Equals(object obj)
        {
            if (obj is Category otherCategory)
            {
                return product == otherCategory.product &&
                    type == otherCategory.type &&
                    topic == otherCategory.topic;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + product.GetHashCode();
                hash = hash * 23 + type.GetHashCode();
                hash = hash * 23 + topic.GetHashCode();
                return hash;
            }
        }

        public int CompareTo(object obj)
        {
            if (obj is Category otherCategory)
                return CompareTo(otherCategory);

            throw new ArgumentException($"Object must be of type {nameof(Category)}");
        }

        public int CompareTo(Category other)
        {
            if (other is null)
                return 1;

            int productComparison = string.Compare(product, other.product, StringComparison.Ordinal);
            if (productComparison != 0)
                return productComparison;

            int typeComparison = type.CompareTo(other.type);
            if (typeComparison != 0)
                return typeComparison;

            return topic.CompareTo(other.topic);
        }

        public static bool operator ==(Category left, Category right) =>
             left.Equals(right);

        public static bool operator !=(Category left, Category right) =>
             !left.Equals(right);

        public static bool operator <(Category left, Category right) =>
             left.CompareTo(right) < 0;

        public static bool operator <=(Category left, Category right) =>
             left.CompareTo(right) <= 0;

        public static bool operator >(Category left, Category right) =>
             left.CompareTo(right) > 0;

        public static bool operator >=(Category left, Category right) =>
             left.CompareTo(right) >= 0;

        public override string ToString() => $"{product}.{type}.{topic}";
    }
}