# Message Queue Program
**To Run this app:**
- Run RabbitMQ
- Run ```MessageQueue.Task1.DataCaptureService```
- Run ```MessageQueue.Task1.MainProcessingService```
- Open  ```..\MessageQueue.Task1.DataCaptureService\bin\Debug\net6.0\SenderFolder\``` and Paste any file to this folder
  - It will store in ```..\MessageQueue.Task1.MainProcessingService\bin\Debug\net6.0\ReceiverFolder\```
