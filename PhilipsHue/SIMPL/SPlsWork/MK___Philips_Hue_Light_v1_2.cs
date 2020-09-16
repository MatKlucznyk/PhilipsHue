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

namespace UserModule_MK___PHILIPS_HUE_LIGHT_V1_2
{
    public class UserModuleClass_MK___PHILIPS_HUE_LIGHT_V1_2 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        
        Crestron.Logos.SplusObjects.DigitalInput GETLIGHT;
        Crestron.Logos.SplusObjects.DigitalInput LIGHTON;
        Crestron.Logos.SplusObjects.DigitalInput LIGHTOFF;
        Crestron.Logos.SplusObjects.DigitalInput LIGHTSENDRGB;
        Crestron.Logos.SplusObjects.AnalogInput LIGHTBRI;
        Crestron.Logos.SplusObjects.AnalogInput LIGHTHUE;
        Crestron.Logos.SplusObjects.AnalogInput LIGHTSAT;
        Crestron.Logos.SplusObjects.AnalogInput LIGHTRED;
        Crestron.Logos.SplusObjects.AnalogInput LIGHTGREEN;
        Crestron.Logos.SplusObjects.AnalogInput LIGHTBLUE;
        Crestron.Logos.SplusObjects.StringInput LIGHTNAMEFROMBRIDGE;
        Crestron.Logos.SplusObjects.DigitalOutput LIGHTISREACHABLE;
        Crestron.Logos.SplusObjects.DigitalOutput LIGHTISON;
        Crestron.Logos.SplusObjects.DigitalOutput LIGHTISOFF;
        Crestron.Logos.SplusObjects.AnalogOutput LIGHTBRIVALUE;
        Crestron.Logos.SplusObjects.AnalogOutput LIGHTHUEVALUE;
        Crestron.Logos.SplusObjects.AnalogOutput LIGHTSATVALUE;
        Crestron.Logos.SplusObjects.AnalogOutput LIGHTREDVALUE;
        Crestron.Logos.SplusObjects.AnalogOutput LIGHTGREENVALUE;
        Crestron.Logos.SplusObjects.AnalogOutput LIGHTBLUEVALUE;
        Crestron.Logos.SplusObjects.StringOutput LIGHTTYPE;
        PhilipsHue.PhilipsHueLight LIGHT;
        StringParameter LIGHT_NAME;
        ushort NTYPE = 0;
        ushort NRGBSEMEPHORE = 0;
        public void NEWLIGHT ( SimplSharpString SNAME , SimplSharpString STYPE , SimplSharpString SSTATE , ushort NBRI , ushort NHUE , ushort NSAT , ushort NRED , ushort NGREEN , ushort NBLUE , SimplSharpString SREACHABLE ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 33;
                LIGHTTYPE  .UpdateValue ( STYPE  .ToString()  ) ; 
                __context__.SourceCodeLine = 34;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (STYPE .ToString() == "Extended color light"))  ) ) 
                    {
                    __context__.SourceCodeLine = 34;
                    NTYPE = (ushort) ( 1 ) ; 
                    }
                
                else 
                    {
                    __context__.SourceCodeLine = 35;
                    NTYPE = (ushort) ( 0 ) ; 
                    }
                
                __context__.SourceCodeLine = 36;
                LIGHTBRIVALUE  .Value = (ushort) ( NBRI ) ; 
                __context__.SourceCodeLine = 37;
                LIGHTHUEVALUE  .Value = (ushort) ( NHUE ) ; 
                __context__.SourceCodeLine = 38;
                LIGHTSATVALUE  .Value = (ushort) ( NSAT ) ; 
                __context__.SourceCodeLine = 39;
                LIGHTREDVALUE  .Value = (ushort) ( NRED ) ; 
                __context__.SourceCodeLine = 40;
                LIGHTGREENVALUE  .Value = (ushort) ( NGREEN ) ; 
                __context__.SourceCodeLine = 41;
                LIGHTBLUEVALUE  .Value = (ushort) ( NBLUE ) ; 
                __context__.SourceCodeLine = 43;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE .ToString() == "True"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 45;
                    LIGHTISON  .Value = (ushort) ( 1 ) ; 
                    __context__.SourceCodeLine = 46;
                    LIGHTISOFF  .Value = (ushort) ( 0 ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 49;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE .ToString() == "False"))  ) ) 
                        { 
                        __context__.SourceCodeLine = 51;
                        LIGHTISON  .Value = (ushort) ( 0 ) ; 
                        __context__.SourceCodeLine = 52;
                        LIGHTISOFF  .Value = (ushort) ( 1 ) ; 
                        } 
                    
                    }
                
                __context__.SourceCodeLine = 55;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SREACHABLE .ToString() == "True"))  ) ) 
                    {
                    __context__.SourceCodeLine = 55;
                    LIGHTISREACHABLE  .Value = (ushort) ( 1 ) ; 
                    }
                
                else 
                    {
                    __context__.SourceCodeLine = 56;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SREACHABLE .ToString() == "False"))  ) ) 
                        {
                        __context__.SourceCodeLine = 56;
                        LIGHTISREACHABLE  .Value = (ushort) ( 0 ) ; 
                        }
                    
                    }
                
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        public void ONTRACE ( SimplSharpString STRACE ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 61;
                /* Trace( "{0}", STRACE  .ToString() ) */ ; 
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        object GETLIGHT_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 67;
                LIGHT . GetLight ( ) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object LIGHTON_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 72;
            if ( Functions.TestForTrue  ( ( LIGHTISREACHABLE  .Value)  ) ) 
                { 
                __context__.SourceCodeLine = 74;
                LIGHT . LightState ( "on", (ushort)( 1 ), (ushort)( 0 ), (ushort)( 0 )) ; 
                __context__.SourceCodeLine = 75;
                LIGHTISON  .Value = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 76;
                LIGHTISOFF  .Value = (ushort) ( 0 ) ; 
                } 
            
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object LIGHTOFF_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 82;
        if ( Functions.TestForTrue  ( ( LIGHTISREACHABLE  .Value)  ) ) 
            { 
            __context__.SourceCodeLine = 84;
            LIGHT . LightState ( "on", (ushort)( 0 ), (ushort)( 0 ), (ushort)( 0 )) ; 
            __context__.SourceCodeLine = 85;
            LIGHTISON  .Value = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 86;
            LIGHTISOFF  .Value = (ushort) ( 1 ) ; 
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object LIGHTSENDRGB_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 92;
        if ( Functions.TestForTrue  ( ( LIGHTISREACHABLE  .Value)  ) ) 
            { 
            __context__.SourceCodeLine = 94;
            LIGHT . LightState ( "xy", (ushort)( LIGHTRED  .UshortValue ), (ushort)( LIGHTGREEN  .UshortValue ), (ushort)( LIGHTBLUE  .UshortValue )) ; 
            __context__.SourceCodeLine = 95;
            LIGHTREDVALUE  .Value = (ushort) ( LIGHTRED  .UshortValue ) ; 
            __context__.SourceCodeLine = 96;
            LIGHTGREENVALUE  .Value = (ushort) ( LIGHTGREEN  .UshortValue ) ; 
            __context__.SourceCodeLine = 97;
            LIGHTBLUEVALUE  .Value = (ushort) ( LIGHTBLUE  .UshortValue ) ; 
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object LIGHTBRI_OnChange_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort NVAL = 0;
        
        
        __context__.SourceCodeLine = 105;
        if ( Functions.TestForTrue  ( ( LIGHTISREACHABLE  .Value)  ) ) 
            { 
            __context__.SourceCodeLine = 107;
            NVAL = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 109;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt (NVAL != LIGHTBRI  .UshortValue))  ) ) 
                { 
                __context__.SourceCodeLine = 111;
                NVAL = (ushort) ( LIGHTBRI  .UshortValue ) ; 
                __context__.SourceCodeLine = 112;
                LIGHT . LightState ( "bri", (ushort)( NVAL ), (ushort)( 0 ), (ushort)( 0 )) ; 
                __context__.SourceCodeLine = 113;
                LIGHTBRIVALUE  .Value = (ushort) ( NVAL ) ; 
                __context__.SourceCodeLine = 114;
                Functions.Delay (  (int) ( 20 ) ) ; 
                __context__.SourceCodeLine = 109;
                } 
            
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object LIGHTHUE_OnChange_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort NVAL = 0;
        
        
        __context__.SourceCodeLine = 123;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NTYPE == 1) ) && Functions.TestForTrue ( LIGHTISREACHABLE  .Value )) ))  ) ) 
            { 
            __context__.SourceCodeLine = 125;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt (NVAL != LIGHTHUE  .UshortValue))  ) ) 
                { 
                __context__.SourceCodeLine = 127;
                NVAL = (ushort) ( LIGHTHUE  .UshortValue ) ; 
                __context__.SourceCodeLine = 128;
                LIGHT . LightState ( "hue", (ushort)( NVAL ), (ushort)( 0 ), (ushort)( 0 )) ; 
                __context__.SourceCodeLine = 129;
                LIGHTHUEVALUE  .Value = (ushort) ( NVAL ) ; 
                __context__.SourceCodeLine = 130;
                Functions.Delay (  (int) ( 20 ) ) ; 
                __context__.SourceCodeLine = 125;
                } 
            
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object LIGHTSAT_OnChange_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort NVAL = 0;
        
        
        __context__.SourceCodeLine = 139;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (NTYPE == 1) ) && Functions.TestForTrue ( LIGHTISREACHABLE  .Value )) ))  ) ) 
            { 
            __context__.SourceCodeLine = 141;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt (NVAL != LIGHTSAT  .UshortValue))  ) ) 
                { 
                __context__.SourceCodeLine = 143;
                NVAL = (ushort) ( LIGHTSAT  .UshortValue ) ; 
                __context__.SourceCodeLine = 144;
                LIGHT . LightState ( "sat", (ushort)( NVAL ), (ushort)( 0 ), (ushort)( 0 )) ; 
                __context__.SourceCodeLine = 145;
                LIGHTSATVALUE  .Value = (ushort) ( NVAL ) ; 
                __context__.SourceCodeLine = 146;
                Functions.Delay (  (int) ( 20 ) ) ; 
                __context__.SourceCodeLine = 141;
                } 
            
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object LIGHTNAMEFROMBRIDGE_OnChange_7 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 153;
        if ( Functions.TestForTrue  ( ( Functions.Length( LIGHTNAMEFROMBRIDGE ))  ) ) 
            {
            __context__.SourceCodeLine = 153;
            LIGHT . Address  =  ( LIGHTNAMEFROMBRIDGE  )  .ToString() ; 
            }
        
        
        
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
        
        __context__.SourceCodeLine = 159;
        if ( Functions.TestForTrue  ( ( Functions.Length( LIGHT_NAME  ))  ) ) 
            {
            __context__.SourceCodeLine = 159;
            LIGHT . Address  =  ( LIGHT_NAME  )  .ToString() ; 
            }
        
        __context__.SourceCodeLine = 160;
        // RegisterDelegate( LIGHT , NEWLIGHTINFO , NEWLIGHT ) 
        LIGHT .newLightInfo  = NEWLIGHT; ; 
        __context__.SourceCodeLine = 161;
        // RegisterDelegate( LIGHT , NEWTRACE , ONTRACE ) 
        LIGHT .newTrace  = ONTRACE; ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    
    GETLIGHT = new Crestron.Logos.SplusObjects.DigitalInput( GETLIGHT__DigitalInput__, this );
    m_DigitalInputList.Add( GETLIGHT__DigitalInput__, GETLIGHT );
    
    LIGHTON = new Crestron.Logos.SplusObjects.DigitalInput( LIGHTON__DigitalInput__, this );
    m_DigitalInputList.Add( LIGHTON__DigitalInput__, LIGHTON );
    
    LIGHTOFF = new Crestron.Logos.SplusObjects.DigitalInput( LIGHTOFF__DigitalInput__, this );
    m_DigitalInputList.Add( LIGHTOFF__DigitalInput__, LIGHTOFF );
    
    LIGHTSENDRGB = new Crestron.Logos.SplusObjects.DigitalInput( LIGHTSENDRGB__DigitalInput__, this );
    m_DigitalInputList.Add( LIGHTSENDRGB__DigitalInput__, LIGHTSENDRGB );
    
    LIGHTISREACHABLE = new Crestron.Logos.SplusObjects.DigitalOutput( LIGHTISREACHABLE__DigitalOutput__, this );
    m_DigitalOutputList.Add( LIGHTISREACHABLE__DigitalOutput__, LIGHTISREACHABLE );
    
    LIGHTISON = new Crestron.Logos.SplusObjects.DigitalOutput( LIGHTISON__DigitalOutput__, this );
    m_DigitalOutputList.Add( LIGHTISON__DigitalOutput__, LIGHTISON );
    
    LIGHTISOFF = new Crestron.Logos.SplusObjects.DigitalOutput( LIGHTISOFF__DigitalOutput__, this );
    m_DigitalOutputList.Add( LIGHTISOFF__DigitalOutput__, LIGHTISOFF );
    
    LIGHTBRI = new Crestron.Logos.SplusObjects.AnalogInput( LIGHTBRI__AnalogSerialInput__, this );
    m_AnalogInputList.Add( LIGHTBRI__AnalogSerialInput__, LIGHTBRI );
    
    LIGHTHUE = new Crestron.Logos.SplusObjects.AnalogInput( LIGHTHUE__AnalogSerialInput__, this );
    m_AnalogInputList.Add( LIGHTHUE__AnalogSerialInput__, LIGHTHUE );
    
    LIGHTSAT = new Crestron.Logos.SplusObjects.AnalogInput( LIGHTSAT__AnalogSerialInput__, this );
    m_AnalogInputList.Add( LIGHTSAT__AnalogSerialInput__, LIGHTSAT );
    
    LIGHTRED = new Crestron.Logos.SplusObjects.AnalogInput( LIGHTRED__AnalogSerialInput__, this );
    m_AnalogInputList.Add( LIGHTRED__AnalogSerialInput__, LIGHTRED );
    
    LIGHTGREEN = new Crestron.Logos.SplusObjects.AnalogInput( LIGHTGREEN__AnalogSerialInput__, this );
    m_AnalogInputList.Add( LIGHTGREEN__AnalogSerialInput__, LIGHTGREEN );
    
    LIGHTBLUE = new Crestron.Logos.SplusObjects.AnalogInput( LIGHTBLUE__AnalogSerialInput__, this );
    m_AnalogInputList.Add( LIGHTBLUE__AnalogSerialInput__, LIGHTBLUE );
    
    LIGHTBRIVALUE = new Crestron.Logos.SplusObjects.AnalogOutput( LIGHTBRIVALUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( LIGHTBRIVALUE__AnalogSerialOutput__, LIGHTBRIVALUE );
    
    LIGHTHUEVALUE = new Crestron.Logos.SplusObjects.AnalogOutput( LIGHTHUEVALUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( LIGHTHUEVALUE__AnalogSerialOutput__, LIGHTHUEVALUE );
    
    LIGHTSATVALUE = new Crestron.Logos.SplusObjects.AnalogOutput( LIGHTSATVALUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( LIGHTSATVALUE__AnalogSerialOutput__, LIGHTSATVALUE );
    
    LIGHTREDVALUE = new Crestron.Logos.SplusObjects.AnalogOutput( LIGHTREDVALUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( LIGHTREDVALUE__AnalogSerialOutput__, LIGHTREDVALUE );
    
    LIGHTGREENVALUE = new Crestron.Logos.SplusObjects.AnalogOutput( LIGHTGREENVALUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( LIGHTGREENVALUE__AnalogSerialOutput__, LIGHTGREENVALUE );
    
    LIGHTBLUEVALUE = new Crestron.Logos.SplusObjects.AnalogOutput( LIGHTBLUEVALUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( LIGHTBLUEVALUE__AnalogSerialOutput__, LIGHTBLUEVALUE );
    
    LIGHTNAMEFROMBRIDGE = new Crestron.Logos.SplusObjects.StringInput( LIGHTNAMEFROMBRIDGE__AnalogSerialInput__, 255, this );
    m_StringInputList.Add( LIGHTNAMEFROMBRIDGE__AnalogSerialInput__, LIGHTNAMEFROMBRIDGE );
    
    LIGHTTYPE = new Crestron.Logos.SplusObjects.StringOutput( LIGHTTYPE__AnalogSerialOutput__, this );
    m_StringOutputList.Add( LIGHTTYPE__AnalogSerialOutput__, LIGHTTYPE );
    
    LIGHT_NAME = new StringParameter( LIGHT_NAME__Parameter__, this );
    m_ParameterList.Add( LIGHT_NAME__Parameter__, LIGHT_NAME );
    
    
    GETLIGHT.OnDigitalPush.Add( new InputChangeHandlerWrapper( GETLIGHT_OnPush_0, false ) );
    LIGHTON.OnDigitalPush.Add( new InputChangeHandlerWrapper( LIGHTON_OnPush_1, false ) );
    LIGHTOFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( LIGHTOFF_OnPush_2, false ) );
    LIGHTSENDRGB.OnDigitalPush.Add( new InputChangeHandlerWrapper( LIGHTSENDRGB_OnPush_3, false ) );
    LIGHTBRI.OnAnalogChange.Add( new InputChangeHandlerWrapper( LIGHTBRI_OnChange_4, true ) );
    LIGHTHUE.OnAnalogChange.Add( new InputChangeHandlerWrapper( LIGHTHUE_OnChange_5, true ) );
    LIGHTSAT.OnAnalogChange.Add( new InputChangeHandlerWrapper( LIGHTSAT_OnChange_6, true ) );
    LIGHTNAMEFROMBRIDGE.OnSerialChange.Add( new InputChangeHandlerWrapper( LIGHTNAMEFROMBRIDGE_OnChange_7, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    LIGHT  = new PhilipsHue.PhilipsHueLight();
    
    
}

public UserModuleClass_MK___PHILIPS_HUE_LIGHT_V1_2 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint GETLIGHT__DigitalInput__ = 0;
const uint LIGHTON__DigitalInput__ = 1;
const uint LIGHTOFF__DigitalInput__ = 2;
const uint LIGHTSENDRGB__DigitalInput__ = 3;
const uint LIGHTBRI__AnalogSerialInput__ = 0;
const uint LIGHTHUE__AnalogSerialInput__ = 1;
const uint LIGHTSAT__AnalogSerialInput__ = 2;
const uint LIGHTRED__AnalogSerialInput__ = 3;
const uint LIGHTGREEN__AnalogSerialInput__ = 4;
const uint LIGHTBLUE__AnalogSerialInput__ = 5;
const uint LIGHTNAMEFROMBRIDGE__AnalogSerialInput__ = 6;
const uint LIGHTISREACHABLE__DigitalOutput__ = 0;
const uint LIGHTISON__DigitalOutput__ = 1;
const uint LIGHTISOFF__DigitalOutput__ = 2;
const uint LIGHTBRIVALUE__AnalogSerialOutput__ = 0;
const uint LIGHTHUEVALUE__AnalogSerialOutput__ = 1;
const uint LIGHTSATVALUE__AnalogSerialOutput__ = 2;
const uint LIGHTREDVALUE__AnalogSerialOutput__ = 3;
const uint LIGHTGREENVALUE__AnalogSerialOutput__ = 4;
const uint LIGHTBLUEVALUE__AnalogSerialOutput__ = 5;
const uint LIGHTTYPE__AnalogSerialOutput__ = 6;
const uint LIGHT_NAME__Parameter__ = 10;

[SplusStructAttribute(-1, true, false)]
public class SplusNVRAM : SplusStructureBase
{

    public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
    
    
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
