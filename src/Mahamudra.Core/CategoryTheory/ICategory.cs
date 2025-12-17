using System;
using System.Collections.Generic;

namespace Mahamudra.Core.CategoryTheory
{
    public interface ICategory<TObject, TMorphism>
    {
        IEnumerable<TObject> Objects { get; }
        TMorphism Morphism(TObject first, TObject second);
        TMorphism Compose(TMorphism morphism2, TMorphism morphism1);
        TMorphism Identity(TObject @object);
    }
}
