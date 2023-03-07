namespace Enums
{

    public enum Trajectory
    {
        Target,
        Predicted
    }

    public enum DataType{
        Coordinate,
        Velocity
    }
    public enum BrowserEvent{
        Opened,
        LoadedTarget,
        LoadedPredicted
    }

    public enum Axis{
        X,
        Y,
        Z,
        XYZ,
        XY,
        YZ,
        XZ
    }

    public enum EnumTags{
        Null,
        TrajectorySphereLabel,
        EndEffectorHand,
        EndEffectorFoot,
        EndEffectorHead, //not used yet
        EndEffectorGeneric, //not used yet or maybe not needed
        Target,
        Predicted
    }
    
}
