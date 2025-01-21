using System;
using UnityEngine;

/**
 * SavableAttribute -> Class
 *
 * @bool CanBeModified -> Authorize Classes To be modified before saving
 */
public class SavableAttribute : Attribute
{
    public bool CanBeModified = true;

    public Type type;

    public SavableAttribute(Type currentType)
    {
        type = currentType;
    }
}
