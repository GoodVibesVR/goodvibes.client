
# GoodVibes.Client
GoodVibes client is the locally running client of the GoodVibes ecosystem. The GoodVibes client uses OSC and websockets to synchronize parameters and control external toys such as Lovense.

## Flow diagram

Our service design allows us to make many kinds of integrations. Below is a few sequence diagrams over how integrations can look.

### GoodVibes Remote Control
Communication between GoodVibes clients is enabled using websockets and can either be controlled directly in the local GoodVibes client or mapped to parameters in OSC. These can be used to either control local resources at the receiving end or sync to the VR Application using OSC.
```mermaid
sequenceDiagram
Local VR Application -->> Local GoodVibes Client: OSC
Local GoodVibes Client->> GoodVibes Servers: Websockets
GoodVibes Servers->> Remote GoodVibes Client: Websockets
Remote GoodVibes Client->> Remote VR Application: OSC
Remote GoodVibes Client-->> Remote Resource: Commands sent to the remote "local resource"
Remote VR Application->> Remote GoodVibes Client: OSC
Remote GoodVibes Client->> GoodVibes Servers: Websockets
GoodVibes Servers->> Local GoodVibes Client: Websockets
Local GoodVibes Client-->> Local VR Application: OSC
```

### External service partner control
Below is an example of a bit more complex flow integrating with an external partner then providing us with the callback information that enables our connectivity to their control application.
```mermaid
sequenceDiagram
VR Application ->> GoodVibes Client: OSC
GoodVibes Client->>GoodVibes Servers: Connect to GoodVibes Servers
GoodVibes Servers->> External Partner: Create remote session
External Partner->> Local Resource: Create remote session
Local Resource->> External Partner: Remote session response
External Partner->>GoodVibes Servers: Remote session response
GoodVibes Servers->>GoodVibes Client: Session connectivity options
External Partner->>GoodVibes Servers: Callback data
GoodVibes Servers->>GoodVibes Client: Callback data
GoodVibes Client-->>Local Resource: Communication with local resource
GoodVibes Client->>VR Application: OSC
```