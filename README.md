This is Proof of Concept (POC) solution that I wrote for CodeCamp SF 2012 talk. Its intend is to compare performance of SQL Azure against Azure Table for one use case.

Code is at POC quality -- no setup, a lot of hard coding etc. Don't just use it to draw conclusions about performance, instead use it as a sample to build you own test for your own use case.

## Use case

I wanted to test performance of the accessing entities by PK. No related data. Typical scenario -- pulling user record (name, address, etc; all stored in one record).

Load has to be substantial to reduce affects of caching. 10M record with 1K dummy record data should be sufficient. Data in Table Storage is partitioned into 1000 partitions.

Querying (to measure performance) is done with random PKs. Whole record is pulled for the storage (SQL or Table). I discard result and neglect if record is not found. 

## How it works

Win form app is used to send messages to worker role. Based on the message, worker role will do certain tasks. Any results of the work (e.g. timings) are recorded to table storage by worker role.

SQL population cannot be effectively scaled out, so one worker role instance that receives sql population command, will do sql population (in series of batches). 
Table storage population scales out easily, so Win control app will send multiple commands and multiple worker instances can work on batches simultaneously. 
Same with measurements: Win app can send multiple measure requests and they will be processed in parallel.

There is no automated workflow built in, you'll have to check when data population completed before starting performance tests.
