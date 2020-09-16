namespace PhilipsHue;
        // class declarations
         class LightDataReceivedEventArgs;
         class GroupDataReceivedEventArgs;
         class ClientEvents;
         class PhilipsHueBridge;
         class PhilipsHueBridgeConfig;
         class PhilipsHueLight;
         class State;
         class Lights;
         class GroupState;
         class Groups;
         class Config;
         class DataDump;
         class User;
         class PhilipsHueGroup;
     class ClientEvents 
    {
        // class delegates

        // class events
        EventHandler OnLightDataReceived ( ClientEvents sender, LightDataReceivedEventArgs e );
        EventHandler OnGroupDataReceived ( ClientEvents sender, GroupDataReceivedEventArgs e );

        // class functions
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

    static class PhilipsHueBridge 
    {
        // class delegates
        delegate FUNCTION SendConfig ( SIMPLSHARPSTRING name , SIMPLSHARPSTRING ver );
        delegate FUNCTION SendError ( SIMPLSHARPSTRING error );
        delegate FUNCTION SendLight ( INTEGER id , SIMPLSHARPSTRING name );
        delegate FUNCTION SendGRoup ( INTEGER id , SIMPLSHARPSTRING name );

        // class events

        // class functions
        static FUNCTION SendConfigHandler ( STRING name , STRING ver );
        static FUNCTION SendErrorHandler ( STRING error );
        static FUNCTION SedndLightHandler ( INTEGER id , STRING name );
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        static STRING IPAddress[];
        static STRING Username[];

        // class properties
        DelegateProperty SendConfig SendConfigFn;
        DelegateProperty SendError SendErrorFn;
        DelegateProperty SendLight SendLightFn;
        DelegateProperty SendGRoup SendGroupFn;
    };

     class PhilipsHueBridgeConfig 
    {
        // class delegates

        // class events

        // class functions
        FUNCTION Link ( STRING user );
        FUNCTION GetLights ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

     class PhilipsHueLight 
    {
        // class delegates
        delegate FUNCTION LightInfo ( SIMPLSHARPSTRING name , SIMPLSHARPSTRING type , SIMPLSHARPSTRING on , INTEGER bri , INTEGER hue , INTEGER sat , INTEGER red , INTEGER green , INTEGER blue , SIMPLSHARPSTRING reachable );
        delegate FUNCTION OnTrace ( SIMPLSHARPSTRING trace );

        // class events
        EventHandler OnLightDataReceived ( PhilipsHueLight sender, LightDataReceivedEventArgs e );

        // class functions
        FUNCTION LightState ( STRING type , INTEGER state1 , INTEGER state2 , INTEGER state3 );
        FUNCTION GetLight ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        DelegateProperty LightInfo newLightInfo;
        DelegateProperty OnTrace newTrace;
        STRING Address[];
    };

     class State 
    {
        // class delegates

        // class events

        // class functions
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        INTEGER bri;
        INTEGER hue;
        INTEGER sat;
    };

     class Lights 
    {
        // class delegates

        // class events

        // class functions
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        State state;
        STRING type[];
        STRING name[];
        STRING modelid[];
    };

     class GroupState 
    {
        // class delegates

        // class events

        // class functions
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
    };

     class Groups 
    {
        // class delegates

        // class events

        // class functions
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        STRING name[];
        STRING type[];
        GroupState state;
        State action;
    };

     class Config 
    {
        // class delegates

        // class events

        // class functions
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        STRING name[];
        STRING swversion[];
    };

     class DataDump 
    {
        // class delegates

        // class events

        // class functions
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        Config config;
    };

     class User 
    {
        // class delegates

        // class events

        // class functions
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        STRING devicetype[];
    };

     class PhilipsHueGroup 
    {
        // class delegates
        delegate FUNCTION GroupInfo ( SIMPLSHARPSTRING name , SIMPLSHARPSTRING type , SIMPLSHARPSTRING allon , SIMPLSHARPSTRING anyOn , SIMPLSHARPSTRING on , INTEGER bri , INTEGER hue , INTEGER sat , INTEGER red , INTEGER green , INTEGER blue );

        // class events
        EventHandler OnGroupDataReceived ( PhilipsHueGroup sender, GroupDataReceivedEventArgs e );

        // class functions
        FUNCTION GroupState ( STRING type , INTEGER state1 , INTEGER state2 , INTEGER state3 );
        FUNCTION GetGroup ();
        SIGNED_LONG_INTEGER_FUNCTION GetHashCode ();
        STRING_FUNCTION ToString ();

        // class variables
        INTEGER __class_id__;

        // class properties
        DelegateProperty GroupInfo newGroupInfo;
        STRING Address[];
    };

