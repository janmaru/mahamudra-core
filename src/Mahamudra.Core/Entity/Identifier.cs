using System;

#nullable enable

namespace Mahamudra.Core.Entity
{
    public readonly struct Identifier<T> : IEquatable<Identifier<T>>  
    {
        public T Id { get; }

        public Identifier(T id) =>
            this.Id = id;

        public override bool Equals(object? obj) =>
            obj is Identifier<T> o && this.Equals(o);

        public bool Equals(Identifier<T> other) => object.Equals(this.Id, other.Id);

        public override int GetHashCode() =>
            HashCode.Combine(this.Id);

        public static bool operator ==(Identifier<T> left, Identifier<T> right) => left.Equals(right);

        public static bool operator !=(Identifier<T> left, Identifier<T> right) => !(left == right);

        public override string ToString() => this.Id?.ToString() ?? string.Empty;
    }
}
