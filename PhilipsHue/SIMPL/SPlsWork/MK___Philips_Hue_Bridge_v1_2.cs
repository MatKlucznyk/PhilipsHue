using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Crestron;
using Crestron.Logos.SplusLibrary;
using Crestron.Logos.SplusObjects;
using Crestron.SimplSharp;
using PhilipsHue;

namespace UserModule_MK___PHILIPS_HUE_BRIDGE_V1_2
{
    public class UserModuleClass_MK___PHILIPS_HUE_BRIDGE_V1_2 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        Crestron.Logos.SplusObjects.DigitalInput LINK;
        Crestron.Logos.SplusObjects.DigitalInput GETLIGHTS;
        Crestron.Logos.SplusObjects.StringInput IPADDRESS;
        Crestron.Logos.SplusObjects.StringInput USERNAME;
        Crestron.Logos.SplusObjects.DigitalOutput ISLINKED;
        Crestron.Logos.SplusObjects.StringOutput BRIDGENAME;
        Crestron.Logos.SplusObjects.StringOutput BRIDGEVERSION;
        Crestron.Logos.SplusObjects.StringOutput STATUS;
        InOutArray<Crestron.Logos.SplusObjects.StringOutput> LIGHTNAME;
        InOutArray<Crestron.Logos.SplusObjects.StringOutput> GROUPNAME;
        PhilipsHue.PhilipsHueBridgeConfig BRIDGECONFIG;
        public void RECEIVECONFIG ( SimplSharpString SNAME , SimplSharpString SVERSION ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 25;
                BRIDGENAME  .UpdateValue ( SNAME  .ToString()  ) ; 
                __context__.SourceCodeLine = 26;
                BRIDGEVERSION  .UpdateValue ( SVERSION  .ToString()  ) ; 
                __context__.SourceCodeLine = 27;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (_SplusNVRAM.NLINKSTAT != 1))  ) ) 
                    { 
                    __context__.SourceCodeLine = 29;
                    _SplusNVRAM.NLINKSTAT = (ushort) ( 1 ) ; 
                    __context__.SourceCodeLine = 30;
                    ISLINKED  .Value = (ushort) ( 1 ) ; 
                    } 
                
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        public void RECEIVEERROR ( SimplSharpString SERROR ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 36;
                STATUS  .UpdateValue ( SERROR  .ToString()  ) ; 
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        public void RECEIVELIGHT ( ushort NID , SimplSharpString SNAME ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 41;
                LIGHTNAME [ NID]  .UpdateValue ( SNAME  .ToString()  ) ; 
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        public void RECEIVEGROUP ( ushort NID , SimplSharpString SNAME ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 46;
                GROUPNAME [ NID]  .UpdateValue ( SNAME  .ToString()  ) ; 
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        object LINK_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 52;
                BRIDGECONFIG . Link ( USERNAME .ToString()) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object GETLIGHTS_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 57;
            BRIDGECONFIG . GetLights ( ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object IPADDRESS_OnChange_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
         PhilipsHueBridge.IPAddress  =( IPADDRESS )  .ToString()  ;  
 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 68;
        // RegisterDelegate( PhilipsHueBridge , SENDCONFIGFN , RECEIVECONFIG ) 
        PhilipsHueBridge .SendConfigFn  = RECEIVECONFIG; ; 
        __context__.SourceCodeLine = 69;
        // RegisterDelegate( PhilipsHueBridge , SENDERRORFN , RECEIVEERROR ) 
        PhilipsHueBridge .SendErrorFn  = RECEIVEERROR; ; 
        __context__.SourceCodeLine = 70;
        // RegisterDelegate( PhilipsHueBridge , SENDLIGHTFN , RECEIVELIGHT ) 
        PhilipsHueBridge .SendLightFn  = RECEIVELIGHT; ; 
        __context__.SourceCodeLine = 71;
        // RegisterDelegate( PhilipsHueBridge , SENDGROUPFN , RECEIVEGROUP ) 
        PhilipsHueBridge .SendGroupFn  = RECEIVEGROUP; ; 
        __context__.SourceCodeLine = 73;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (_SplusNVRAM.NLINKSTAT == 1))  ) ) 
            {
            __context__.SourceCodeLine = 73;
            ISLINKED  .Value = (ushort) ( 1 ) ; 
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    
    LINK = new Crestron.Logos.SplusObjects.DigitalInput( LINK__DigitalInput__, this );
    m_DigitalInputList.Add( LINK__DigitalInput__, LINK );
    
    GETLIGHTS = new Crestron.Logos.SplusObjects.DigitalInput( GETLIGHTS__DigitalInput__, this );
    m_DigitalInputList.Add( GETLIGHTS__DigitalInput__, GETLIGHTS );
    
    ISLINKED = new Crestron.Logos.SplusObjects.DigitalOutput( ISLINKED__DigitalOutput__, this );
    m_DigitalOutputList.Add( ISLINKED__DigitalOutput__, ISLINKED );
    
    IPADDRESS = new Crestron.Logos.SplusObjects.StringInput( IPADDRESS__AnalogSerialInput__, 16, this );
    m_StringInputList.Add( IPADDRESS__AnalogSerialInput__, IPADDRESS );
    
    USERNAME = new Crestron.Logos.SplusObjects.StringInput( USERNAME__AnalogSerialInput__, 1000, this );
    m_StringInputList.Add( USERNAME__AnalogSerialInput__, USERNAME );
    
    BRIDGENAME = new Crestron.Logos.SplusObjects.StringOutput( BRIDGENAME__AnalogSerialOutput__, this );
    m_StringOutputList.Add( BRIDGENAME__AnalogSerialOutput__, BRIDGENAME );
    
    BRIDGEVERSION = new Crestron.Logos.SplusObjects.StringOutput( BRIDGEVERSION__AnalogSerialOutput__, this );
    m_StringOutputList.Add( BRIDGEVERSION__AnalogSerialOutput__, BRIDGEVERSION );
    
    STATUS = new Crestron.Logos.SplusObjects.StringOutput( STATUS__AnalogSerialOutput__, this );
    m_StringOutputList.Add( STATUS__AnalogSerialOutput__, STATUS );
    
    LIGHTNAME = new InOutArray<StringOutput>( 50, this );
    for( uint i = 0; i < 50; i++ )
    {
        LIGHTNAME[i+1] = new Crestron.Logos.SplusObjects.StringOutput( LIGHTNAME__AnalogSerialOutput__ + i, this );
        m_StringOutputList.Add( LIGHTNAME__AnalogSerialOutput__ + i, LIGHTNAME[i+1] );
    }
    
    GROUPNAME = new InOutArray<StringOutput>( 50, this );
    for( uint i = 0; i < 50; i++ )
    {
        GROUPNAME[i+1] = new Crestron.Logos.SplusObjects.StringOutput( GROUPNAME__AnalogSerialOutput__ + i, this );
        m_StringOutputList.Add( GROUPNAME__AnalogSerialOutput__ + i, GROUPNAME[i+1] );
    }
    
    
    LINK.OnDigitalPush.Add( new InputChangeHandlerWrapper( LINK_OnPush_0, false ) );
    GETLIGHTS.OnDigitalPush.Add( new InputChangeHandlerWrapper( GETLIGHTS_OnPush_1, false ) );
    IPADDRESS.OnSerialChange.Add( new InputChangeHandlerWrapper( IPADDRESS_OnChange_2, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    BRIDGECONFIG  = new PhilipsHue.PhilipsHueBridgeConfig();
    
    
}

public UserModuleClass_MK___PHILIPS_HUE_BRIDGE_V1_2 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint LINK__DigitalInput__ = 0;
const uint GETLIGHTS__DigitalInput__ = 1;
const uint IPADDRESS__AnalogSerialInput__ = 0;
const uint USERNAME__AnalogSerialInput__ = 1;
const uint ISLINKED__DigitalOutput__ = 0;
const uint BRIDGENAME__AnalogSerialOutput__ = 0;
const uint BRIDGEVERSION__AnalogSerialOutput__ = 1;
const uint STATUS__AnalogSerialOutput__ = 2;
const uint LIGHTNAME__AnalogSerialOutput__ = 3;
const uint GROUPNAME__AnalogSerialOutput__ = 53;

[SplusStructAttribute(-1, true, false)]
public class SplusNVRAM : SplusStructureBase
{

    public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
    
    [SplusStructAttribute(0, false, true)]
            public ushort NLINKSTAT = 0;
            
}

SplusNVRAM _SplusNVRAM = null;

public class __CEvent__ : CEvent
{
    public __CEvent__() {}
    public void Close() { base.Close(); }
    public int Reset() { return base.Reset() ? 1 : 0; }
    public int Set() { return base.Set() ? 1 : 0; }
    public int Wait( int timeOutInMs ) { return base.Wait( timeOutInMs ) ? 1 : 0; }
}
public class __CMutex__ : CMutex
{
    public __CMutex__() {}
    public void Close() { base.Close(); }
    public void ReleaseMutex() { base.ReleaseMutex(); }
    public int WaitForMutex() { return base.WaitForMutex() ? 1 : 0; }
}
 public int IsNull( object obj ){ return (obj == null) ? 1 : 0; }
}


}
