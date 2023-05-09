# Recommend database for Triage-Plus

**Triage-Plus** is a multi-tenant application that allows tenants to customize their data by adding extra fields into an object. The extra fields can be of different types. So we need a database:
+ Can easy to add or modify extra fields for different tenants
+ Has good performance
+ Scalability
Of course we still need other things like Availability, Cost...However, I assume that both NoSQL and relational databases can provide these features to some extent.
Since data of **Triage-Plus** is dynamic and frequently changes. I recommend using a NoSQL database like MongoDB for this system.