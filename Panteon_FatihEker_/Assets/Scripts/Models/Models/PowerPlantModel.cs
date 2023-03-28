public class PowerPlantModel : BaseModel {

    private FactoryBase _factoryBase;
    

    public override void SetBuildingModel(FactoryBase factoryBase) {
        _factoryBase = factoryBase;
    }

    public override FactoryBase GetFactoryBase() {
        return _factoryBase;
    }

    public void MarkDestination(bool isUserInput) {
        
    }
}
