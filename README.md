# .NET 6 Demo 
now running on .NET 6 RC1
- Custom RabbitMQ ILogger Provider
- RabbitMQ.Client Publish
- RabbitMQ.Client Consume

---

## Custom RabbitMQ ILogger Provider
Check blog if you want.
- Add RabbitMQLogger & RabbitMQLoggerProvider [RabbitMQLoggerProvider.cs](https://github.com/died/NET-6-Demo/blob/6ac2a6404213702d921e47d892a57aed439328fc/Net6%20Demo/Providers/RabbitMQLoggerProvider.cs)  
- Add customer logger extension [ConfigExtension.cs](https://github.com/died/NET-6-Demo/blob/6ac2a6404213702d921e47d892a57aed439328fc/Net6%20Demo/Extensions/ConfigExtension.cs)  
- Add into ILogger in [Program.cs](https://github.com/died/NET-6-Demo/blob/6ac2a6404213702d921e47d892a57aed439328fc/Net6%20Demo/Program.cs#L9-L13)

---

## RabbitMQ.Client Publish
- nuget package [RabbitMQ.Client](https://www.nuget.org/packages/RabbitMQ.Client)
- Add [RabbitMQHelper.cs](https://github.com/died/NET-6-Demo/blob/6ac2a6404213702d921e47d892a57aed439328fc/Net6%20Demo/Helpers/RabbitMQHelper.cs) handle connection and publish
- DI it in [Program.cs](https://github.com/died/NET-6-Demo/blob/6ac2a6404213702d921e47d892a57aed439328fc/Net6%20Demo/Program.cs#L20)

### Test publish in controller
- LogInformation [here]
- LogError [here]
---

## RabbitMQ.Client Consume
I am lazy to create frontend to read consumed queue, so use background worker, write in console.  
- Add BackgroundTask to handle worker [BackgroundTask.cs](https://github.com/died/NET-6-Demo/blob/6ac2a6404213702d921e47d892a57aed439328fc/Net6%20Demo/Workers/BackgroundTask.cs)  
- Add ConsumeWorker [ConsumeWorker.cs](https://github.com/died/NET-6-Demo/blob/6ac2a6404213702d921e47d892a57aed439328fc/Net6%20Demo/Workers/ConsumeWorker.cs)  

You can join those in one worker if you only have one.
- DI it in [Program.cs](https://github.com/died/NET-6-Demo/blob/6ac2a6404213702d921e47d892a57aed439328fc/Net6%20Demo/Program.cs#L17-L18)

---

## Result  

![img](https://i.imgur.com/WDvnENN.png)  

![img](https://i.imgur.com/yR2suhu.png)  

![img](https://i.imgur.com/62zNq4R.png)  

