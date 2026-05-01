# PhilipsHue

Crestron SIMPL module suite for controlling Philips Hue lighting systems. Provides comprehensive bridge connectivity, individual light control, and group management capabilities integrated with Crestron systems.

> **Note:**  
> All light and group operations support thread-safe concurrent access patterns.  
> Real-time device state updates and comprehensive control feedback.

---

## ⚙️ Module Overview

The PhilipsHue module suite provides comprehensive control and monitoring of Philips Hue lighting systems within Crestron environments with the following capabilities:

- **Bridge Connectivity** - Connection management to Philips Hue Bridge with authentication and linking
- **Light Control** - Individual light control with on/off, brightness, and color adjustment
- **Color Management** - RGB color control and Hue/Saturation adjustment for extended color lights
- **Group Management** - Control multiple lights as groups with synchronized state management
- **Light Discovery** - Automatic discovery and enumeration of available lights and groups from bridge
- **Real-Time Monitoring** - Device state feedback including reachability and on/off status
- **Thread-Safe Operations** - Concurrent light and group access with smooth transitions and thread-safe implementations

---

## 🗂 Required Files

The module suite is available for use in Crestron systems:

* `Philips Hue Bridge v2.0.0.usp` - Bridge connectivity and management module (SIMPL+ source)
* `Philips Hue Bridge v2.0.0.ush` - Bridge module header
* `Philips Hue Light v2.0.0.usp` - Individual light control module (SIMPL+ source)
* `Philips Hue Light v2.0.0.ush` - Light module header
* `Philips Hue Group v2.0.0.usp` - Group lighting control module (SIMPL+ source)
* `Philips Hue Group v2.0.0.ush` - Group module header
* `PhilipsHue.clz` - Class library containing underlying bridge communication and device management support
