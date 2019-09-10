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

namespace UserModule_MK___PHILIPS_HUE_GROUP_V1_2
{
    public class UserModuleClass_MK___PHILIPS_HUE_GROUP_V1_2 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        
        Crestron.Logos.SplusObjects.DigitalInput GETGROUP;
        Crestron.Logos.SplusObjects.DigitalInput GROUPON;
        Crestron.Logos.SplusObjects.DigitalInput GROUPOFF;
        Crestron.Logos.SplusObjects.DigitalInput GROUPSENDRGB;
        Crestron.Logos.SplusObjects.AnalogInput GROUPBRI;
        Crestron.Logos.SplusObjects.AnalogInput GROUPHUE;
        Crestron.Logos.SplusObjects.AnalogInput GROUPSAT;
        Crestron.Logos.SplusObjects.AnalogInput GROUPRED;
        Crestron.Logos.SplusObjects.AnalogInput GROUPGREEN;
        Crestron.Logos.SplusObjects.AnalogInput GROUPBLUE;
        Crestron.Logos.SplusObjects.StringInput GROUPNAMEFROMBRIDGE;
        Crestron.Logos.SplusObjects.DigitalOutput GROUPISON;
        Crestron.Logos.SplusObjects.DigitalOutput GROUPISOFF;
        Crestron.Logos.SplusObjects.DigitalOutput GROUPALLON;
        Crestron.Logos.SplusObjects.DigitalOutput GROUPANYON;
        Crestron.Logos.SplusObjects.AnalogOutput GROUPBRIVALUE;
        Crestron.Logos.SplusObjects.AnalogOutput GROUPHUEVALUE;
        Crestron.Logos.SplusObjects.AnalogOutput GROUPSATVALUE;
        Crestron.Logos.SplusObjects.AnalogOutput GROUPREDVALUE;
        Crestron.Logos.SplusObjects.AnalogOutput GROUPGREENVALUE;
        Crestron.Logos.SplusObjects.AnalogOutput GROUPBLUEVALUE;
        Crestron.Logos.SplusObjects.StringOutput GROUPTYPE;
        PhilipsHue.PhilipsHueGroup GROUP;
        StringParameter GROUP_NAME;
        ushort NTYPE = 0;
        public void NEWGROUP ( SimplSharpString SNAME , SimplSharpString STYPE , SimplSharpString SALLON , SimplSharpString SANYON , SimplSharpString SSTATE , ushort NBRI , ushort NHUE , ushort NSAT , ushort NRED , ushort NGREEN , ushort NBLUE ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusSimplSharpDelegateThreadStartCode();
                
                __context__.SourceCodeLine = 32;
                GROUPTYPE  .UpdateValue ( STYPE  .ToString()  ) ; 
                __context__.SourceCodeLine = 33;
                GROUPBRIVALUE  .Value = (ushort) ( NBRI ) ; 
                __context__.SourceCodeLine = 34;
                GROUPHUEVALUE  .Value = (ushort) ( NHUE ) ; 
                __context__.SourceCodeLine = 35;
                GROUPSATVALUE  .Value = (ushort) ( NSAT ) ; 
                __context__.SourceCodeLine = 36;
                GROUPREDVALUE  .Value = (ushort) ( NRED ) ; 
                __context__.SourceCodeLine = 37;
                GROUPGREENVALUE  .Value = (ushort) ( NGREEN ) ; 
                __context__.SourceCodeLine = 38;
                GROUPBLUEVALUE  .Value = (ushort) ( NBLUE ) ; 
                __context__.SourceCodeLine = 41;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SALLON .ToString() == "True"))  ) ) 
                    {
                    __context__.SourceCodeLine = 41;
                    GROUPALLON  .Value = (ushort) ( 1 ) ; 
                    }
                
                else 
                    {
                    __context__.SourceCodeLine = 42;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SALLON .ToString() == "False"))  ) ) 
                        {
                        __context__.SourceCodeLine = 42;
                        GROUPALLON  .Value = (ushort) ( 0 ) ; 
                        }
                    
                    }
                
                __context__.SourceCodeLine = 44;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SANYON .ToString() == "True"))  ) ) 
                    {
                    __context__.SourceCodeLine = 44;
                    GROUPANYON  .Value = (ushort) ( 1 ) ; 
                    }
                
                else 
                    {
                    __context__.SourceCodeLine = 45;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SANYON .ToString() == "False"))  ) ) 
                        {
                        __context__.SourceCodeLine = 45;
                        GROUPANYON  .Value = (ushort) ( 0 ) ; 
                        }
                    
                    }
                
                __context__.SourceCodeLine = 47;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE .ToString() == "True"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 49;
                    GROUPISON  .Value = (ushort) ( 1 ) ; 
                    __context__.SourceCodeLine = 50;
                    GROUPISOFF  .Value = (ushort) ( 0 ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 53;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE .ToString() == "False"))  ) ) 
                        { 
                        __context__.SourceCodeLine = 55;
                        GROUPISON  .Value = (ushort) ( 0 ) ; 
                        __context__.SourceCodeLine = 56;
                        GROUPISOFF  .Value = (ushort) ( 1 ) ; 
                        } 
                    
                    }
                
                
                
            }
            finally { ObjectFinallyHandler(); }
            }
            
        object GROUPON_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 63;
                GROUP . GroupState ( "on", (ushort)( 1 ), (ushort)( 0 ), (ushort)( 0 )) ; 
                __context__.SourceCodeLine = 64;
                GROUPISON  .Value = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 65;
                GROUPISOFF  .Value = (ushort) ( 0 ) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object GROUPOFF_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 70;
            GROUP . GroupState ( "on", (ushort)( 0 ), (ushort)( 0 ), (ushort)( 0 )) ; 
            __context__.SourceCodeLine = 71;
            GROUPISON  .Value = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 72;
            GROUPISOFF  .Value = (ushort) ( 1 ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object GROUPSENDRGB_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 77;
        GROUP . GroupState ( "xy", (ushort)( GROUPRED  .UshortValue ), (ushort)( GROUPGREEN  .UshortValue ), (ushort)( GROUPBLUE  .UshortValue )) ; 
        __context__.SourceCodeLine = 78;
        GROUPREDVALUE  .Value = (ushort) ( GROUPRED  .UshortValue ) ; 
        __context__.SourceCodeLine = 79;
        GROUPGREENVALUE  .Value = (ushort) ( GROUPGREEN  .UshortValue ) ; 
        __context__.SourceCodeLine = 80;
        GROUPBLUEVALUE  .Value = (ushort) ( GROUPBLUE  .UshortValue ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object GROUPBRI_OnChange_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort NVAL = 0;
        
        
        __context__.SourceCodeLine = 87;
        NVAL = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 89;
        while ( Functions.TestForTrue  ( ( Functions.BoolToInt (NVAL != GROUPBRI  .UshortValue))  ) ) 
            { 
            __context__.SourceCodeLine = 91;
            NVAL = (ushort) ( GROUPBRI  .UshortValue ) ; 
            __context__.SourceCodeLine = 92;
            GROUP . GroupState ( "bri", (ushort)( NVAL ), (ushort)( 0 ), (ushort)( 0 )) ; 
            __context__.SourceCodeLine = 93;
            Functions.Delay (  (int) ( 20 ) ) ; 
            __context__.SourceCodeLine = 89;
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object GROUPHUE_OnChange_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort NVAL = 0;
        
        
        __context__.SourceCodeLine = 101;
        while ( Functions.TestForTrue  ( ( Functions.BoolToInt (NVAL != GROUPHUE  .UshortValue))  ) ) 
            { 
            __context__.SourceCodeLine = 103;
            NVAL = (ushort) ( GROUPHUE  .UshortValue ) ; 
            __context__.SourceCodeLine = 104;
            GROUP . GroupState ( "hue", (ushort)( NVAL ), (ushort)( 0 ), (ushort)( 0 )) ; 
            __context__.SourceCodeLine = 105;
            Functions.Delay (  (int) ( 20 ) ) ; 
            __context__.SourceCodeLine = 101;
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object GROUPSAT_OnChange_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort NVAL = 0;
        
        
        __context__.SourceCodeLine = 113;
        while ( Functions.TestForTrue  ( ( Functions.BoolToInt (NVAL != GROUPSAT  .UshortValue))  ) ) 
            { 
            __context__.SourceCodeLine = 115;
            NVAL = (ushort) ( GROUPSAT  .UshortValue ) ; 
            __context__.SourceCodeLine = 116;
            GROUP . GroupState ( "sat", (ushort)( NVAL ), (ushort)( 0 ), (ushort)( 0 )) ; 
            __context__.SourceCodeLine = 117;
            Functions.Delay (  (int) ( 20 ) ) ; 
            __context__.SourceCodeLine = 113;
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object GROUPNAMEFROMBRIDGE_OnChange_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 123;
        if ( Functions.TestForTrue  ( ( Functions.Length( GROUPNAMEFROMBRIDGE ))  ) ) 
            {
            __context__.SourceCodeLine = 123;
            GROUP . Address  =  ( GROUPNAMEFROMBRIDGE  )  .ToString() ; 
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
        
        __context__.SourceCodeLine = 129;
        if ( Functions.TestForTrue  ( ( Functions.Length( GROUP_NAME  ))  ) ) 
            {
            __context__.SourceCodeLine = 129;
            GROUP . Address  =  ( GROUP_NAME  )  .ToString() ; 
            }
        
        __context__.SourceCodeLine = 130;
        // RegisterDelegate( GROUP , NEWGROUPINFO , NEWGROUP ) 
        GROUP .newGroupInfo  = NEWGROUP; ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    SocketInfo __socketinfo__ = new SocketInfo( 1, this );
    InitialParametersClass.ResolveHostName = __socketinfo__.ResolveHostName;
    _SplusNVRAM = new SplusNVRAM( this );
    
    GETGROUP = new Crestron.Logos.SplusObjects.DigitalInput( GETGROUP__DigitalInput__, this );
    m_DigitalInputList.Add( GETGROUP__DigitalInput__, GETGROUP );
    
    GROUPON = new Crestron.Logos.SplusObjects.DigitalInput( GROUPON__DigitalInput__, this );
    m_DigitalInputList.Add( GROUPON__DigitalInput__, GROUPON );
    
    GROUPOFF = new Crestron.Logos.SplusObjects.DigitalInput( GROUPOFF__DigitalInput__, this );
    m_DigitalInputList.Add( GROUPOFF__DigitalInput__, GROUPOFF );
    
    GROUPSENDRGB = new Crestron.Logos.SplusObjects.DigitalInput( GROUPSENDRGB__DigitalInput__, this );
    m_DigitalInputList.Add( GROUPSENDRGB__DigitalInput__, GROUPSENDRGB );
    
    GROUPISON = new Crestron.Logos.SplusObjects.DigitalOutput( GROUPISON__DigitalOutput__, this );
    m_DigitalOutputList.Add( GROUPISON__DigitalOutput__, GROUPISON );
    
    GROUPISOFF = new Crestron.Logos.SplusObjects.DigitalOutput( GROUPISOFF__DigitalOutput__, this );
    m_DigitalOutputList.Add( GROUPISOFF__DigitalOutput__, GROUPISOFF );
    
    GROUPALLON = new Crestron.Logos.SplusObjects.DigitalOutput( GROUPALLON__DigitalOutput__, this );
    m_DigitalOutputList.Add( GROUPALLON__DigitalOutput__, GROUPALLON );
    
    GROUPANYON = new Crestron.Logos.SplusObjects.DigitalOutput( GROUPANYON__DigitalOutput__, this );
    m_DigitalOutputList.Add( GROUPANYON__DigitalOutput__, GROUPANYON );
    
    GROUPBRI = new Crestron.Logos.SplusObjects.AnalogInput( GROUPBRI__AnalogSerialInput__, this );
    m_AnalogInputList.Add( GROUPBRI__AnalogSerialInput__, GROUPBRI );
    
    GROUPHUE = new Crestron.Logos.SplusObjects.AnalogInput( GROUPHUE__AnalogSerialInput__, this );
    m_AnalogInputList.Add( GROUPHUE__AnalogSerialInput__, GROUPHUE );
    
    GROUPSAT = new Crestron.Logos.SplusObjects.AnalogInput( GROUPSAT__AnalogSerialInput__, this );
    m_AnalogInputList.Add( GROUPSAT__AnalogSerialInput__, GROUPSAT );
    
    GROUPRED = new Crestron.Logos.SplusObjects.AnalogInput( GROUPRED__AnalogSerialInput__, this );
    m_AnalogInputList.Add( GROUPRED__AnalogSerialInput__, GROUPRED );
    
    GROUPGREEN = new Crestron.Logos.SplusObjects.AnalogInput( GROUPGREEN__AnalogSerialInput__, this );
    m_AnalogInputList.Add( GROUPGREEN__AnalogSerialInput__, GROUPGREEN );
    
    GROUPBLUE = new Crestron.Logos.SplusObjects.AnalogInput( GROUPBLUE__AnalogSerialInput__, this );
    m_AnalogInputList.Add( GROUPBLUE__AnalogSerialInput__, GROUPBLUE );
    
    GROUPBRIVALUE = new Crestron.Logos.SplusObjects.AnalogOutput( GROUPBRIVALUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( GROUPBRIVALUE__AnalogSerialOutput__, GROUPBRIVALUE );
    
    GROUPHUEVALUE = new Crestron.Logos.SplusObjects.AnalogOutput( GROUPHUEVALUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( GROUPHUEVALUE__AnalogSerialOutput__, GROUPHUEVALUE );
    
    GROUPSATVALUE = new Crestron.Logos.SplusObjects.AnalogOutput( GROUPSATVALUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( GROUPSATVALUE__AnalogSerialOutput__, GROUPSATVALUE );
    
    GROUPREDVALUE = new Crestron.Logos.SplusObjects.AnalogOutput( GROUPREDVALUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( GROUPREDVALUE__AnalogSerialOutput__, GROUPREDVALUE );
    
    GROUPGREENVALUE = new Crestron.Logos.SplusObjects.AnalogOutput( GROUPGREENVALUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( GROUPGREENVALUE__AnalogSerialOutput__, GROUPGREENVALUE );
    
    GROUPBLUEVALUE = new Crestron.Logos.SplusObjects.AnalogOutput( GROUPBLUEVALUE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( GROUPBLUEVALUE__AnalogSerialOutput__, GROUPBLUEVALUE );
    
    GROUPNAMEFROMBRIDGE = new Crestron.Logos.SplusObjects.StringInput( GROUPNAMEFROMBRIDGE__AnalogSerialInput__, 255, this );
    m_StringInputList.Add( GROUPNAMEFROMBRIDGE__AnalogSerialInput__, GROUPNAMEFROMBRIDGE );
    
    GROUPTYPE = new Crestron.Logos.SplusObjects.StringOutput( GROUPTYPE__AnalogSerialOutput__, this );
    m_StringOutputList.Add( GROUPTYPE__AnalogSerialOutput__, GROUPTYPE );
    
    GROUP_NAME = new StringParameter( GROUP_NAME__Parameter__, this );
    m_ParameterList.Add( GROUP_NAME__Parameter__, GROUP_NAME );
    
    
    GROUPON.OnDigitalPush.Add( new InputChangeHandlerWrapper( GROUPON_OnPush_0, false ) );
    GROUPOFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( GROUPOFF_OnPush_1, false ) );
    GROUPSENDRGB.OnDigitalPush.Add( new InputChangeHandlerWrapper( GROUPSENDRGB_OnPush_2, false ) );
    GROUPBRI.OnAnalogChange.Add( new InputChangeHandlerWrapper( GROUPBRI_OnChange_3, true ) );
    GROUPHUE.OnAnalogChange.Add( new InputChangeHandlerWrapper( GROUPHUE_OnChange_4, true ) );
    GROUPSAT.OnAnalogChange.Add( new InputChangeHandlerWrapper( GROUPSAT_OnChange_5, true ) );
    GROUPNAMEFROMBRIDGE.OnSerialChange.Add( new InputChangeHandlerWrapper( GROUPNAMEFROMBRIDGE_OnChange_6, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    GROUP  = new PhilipsHue.PhilipsHueGroup();
    
    
}

public UserModuleClass_MK___PHILIPS_HUE_GROUP_V1_2 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint GETGROUP__DigitalInput__ = 0;
const uint GROUPON__DigitalInput__ = 1;
const uint GROUPOFF__DigitalInput__ = 2;
const uint GROUPSENDRGB__DigitalInput__ = 3;
const uint GROUPBRI__AnalogSerialInput__ = 0;
const uint GROUPHUE__AnalogSerialInput__ = 1;
const uint GROUPSAT__AnalogSerialInput__ = 2;
const uint GROUPRED__AnalogSerialInput__ = 3;
const uint GROUPGREEN__AnalogSerialInput__ = 4;
const uint GROUPBLUE__AnalogSerialInput__ = 5;
const uint GROUPNAMEFROMBRIDGE__AnalogSerialInput__ = 6;
const uint GROUPISON__DigitalOutput__ = 0;
const uint GROUPISOFF__DigitalOutput__ = 1;
const uint GROUPALLON__DigitalOutput__ = 2;
const uint GROUPANYON__DigitalOutput__ = 3;
const uint GROUPBRIVALUE__AnalogSerialOutput__ = 0;
const uint GROUPHUEVALUE__AnalogSerialOutput__ = 1;
const uint GROUPSATVALUE__AnalogSerialOutput__ = 2;
const uint GROUPREDVALUE__AnalogSerialOutput__ = 3;
const uint GROUPGREENVALUE__AnalogSerialOutput__ = 4;
const uint GROUPBLUEVALUE__AnalogSerialOutput__ = 5;
const uint GROUPTYPE__AnalogSerialOutput__ = 6;
const uint GROUP_NAME__Parameter__ = 10;

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
