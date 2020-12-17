using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class ExampleBehavior : MonoBehaviour
{
    // This will store the string value
    // [StringInList("Cat", "Dog")] public string Animal;
    // This will store the index of the array value
    // [StringInList("John", "Jack", "Jim")] public int PersonID;

    // Showing a list of loaded scenes
    // [StringInList(typeof(PropertyDrawersHelper) , "AllNames")]
    [ValueDropdown("GetIDList")]
    public int ID;

    private IEnumerable GetIDList()
    {
        return PropertyDrawersHelper
               .AllIDs()
               .Select(id => new ValueDropdownItem(PropertyDrawersHelper.GetName(id) , id));
    }
}