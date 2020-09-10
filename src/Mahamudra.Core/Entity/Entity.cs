using System;

namespace Mahamudra.Core.Entity
{
    public abstract class Entity<Identifier>
    {
        public virtual Identifier Id { get; protected set; }

        public abstract override string ToString();

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity<Identifier>;
 
            // If parameter is null, return false.
            if (Object.ReferenceEquals(obj, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != obj.GetType())
            {
                return false;
            } 
   
            return Id.Equals(compareTo.Id);
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// The hash code should not change during the lifetime of an object. Therefore the fields which are used to calculate the hash code must be immutable.</returns>
        public override int GetHashCode()
        { 
            unchecked  
            { 
                int hash = 13;
                hash = (hash * 7) + Id.GetHashCode(); 
                return hash;
            }
        }

        public static bool operator ==(Entity<Identifier> a, Entity<Identifier> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity<Identifier> a, Entity<Identifier> b)
        {
            return !(a == b);
        }
    }
}
