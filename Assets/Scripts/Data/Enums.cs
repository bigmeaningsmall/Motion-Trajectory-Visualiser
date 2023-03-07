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

    public enum Effector{
        Hand,
        Foot,
        Head,
        Generic
    }

    public enum Tags{
        Null,
        TrajectorySphereLabel,
        EndEffectorHand,
        EndEffectorFoot,
        Target,
        Predicted
    }
    
}
