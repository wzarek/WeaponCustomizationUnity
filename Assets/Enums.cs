using System.ComponentModel;
namespace Scripts
{
    enum SceneState
    {
        [Description("INSPECT")]
        Inspect,
        [Description("MODIFY")]
        Modify
    }

    enum Sight
    {
        [Description("S0")]
        Default,
        [Description("S1")]
        S1
    }

    enum AnimationState
    {
        Changing,
        Finished
    }
}