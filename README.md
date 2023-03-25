# Microservices -  Food Online

<p align="center"> 
    <ol>
    <li>Application contains multiple loosely coupled microservices</li>
    <li>Individual Technology Stack</li>
    <li>Communication between microservices with service bus</li>
    <li>Separation of Concern</li>
    </ol> 
</p>

---

## Communication (SYNC/ASYNC)

### Syncronous Communication
<p align="center">
  <ol>
    <li>Synchronous communication was never a problem in relatively small monolithic applications because it is a very simple concept to reason about.</li> 
    <li>The client sends a request to the server, and the server responds to the client.</li>
    <li>An advantage of synchronous communication is that the service receives an acknowledgement that the request was received, and the corresponding action was   
    executed.</li>
  </ol>  
 </p>
 
 ### Asyncronous Communication
 
 <p align="center">
  <ol>  
    <li>When using asynchronous communication, the calling service does not wait for a response from the called service.</li>
    <li>Asynchronous communication also allows the possibility of One-To-Many communication, where a client can send a message to multiple services at once.</li>   
  </ol>
</p>

---

### Rabit MQ implementation points

1. Install below software on local machine.
  <ul>
    <li>ErLang: https://www.erlang.org/downloads</li>
    <li>RabitMQ: https://www.rabbitmq.com/download.html</li>
  </ul>
2. Open Command prompt and navigate to below path </br>
    C:\Program Files\RabbitMQ Server\rabbitmq_server-3.11.10\sbin</br>
3. Then run below command</br>
    rabbitmq-plugins enable rabbitmq_management</br>
4. Open browser and navigate to the url to open rabitmq in browser- http://localhost:15672</br>
5. Install nuget package RabbitMQ Client into required service</br>
