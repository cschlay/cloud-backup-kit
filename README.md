# Cloud Backup Kit

Many cloud providers offer built-in managed backups, however it has not helped me to sleep better.
There also exists third-party service providers that manage your off-site backups.

In this repository you can find a collection of tools to do it yourself.
You will need at least a backup machine and a disk to put the data in e.g.
a [Raspberry Pi](https://www.raspberrypi.com/) and SSD/HDD disk is sufficient for small applications.

## Off-Site Worker

This application runs locally, it will expect an open connection to a SQL database.
It does not use a queueing services and it will poll the database periodically.

_Note: You should use a separate database to keep backup information as it is exposed to internet, this approach reduces
the need to have a separate API server._
