using UnityEngine;
using System.Collections.Generic;
public abstract class BaseModel : MonoBehaviour {
    public abstract void SetBuildingModel(FactoryBase factoryBase);
    public abstract FactoryBase GetFactoryBase();

}