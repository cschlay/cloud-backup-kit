# Cloud Backup Kit

Many cloud providers offer built-in managed backups, however it has not helped me to sleep better.
There also exists third-party service providers that manage your off-site backups.

In this repository you can find a collection of tools to do it yourself.
You will need at least a backup machine and a disk to put the data in e.g.
a [Raspberry Pi](https://www.raspberrypi.com/) and SSD/HDD disk is sufficient for small applications.

## Installation

TODOS:

- LocalStack
- Configuration

## Features

### Off-Site Worker

This application runs locally, it will expect an open connection to a SQL database.
It does not use a queueing services and will poll the database periodically.

The files in object storages will be mapped as follows assuming the bucket `car-erp`

- `/cars/id-1/thumbnail.png` to `/backups/car-erp/cars/id-1/thumbnail/v1.gzip`
- `/cars/1ff7f0ca-4ee5-4cfe-ad1a-93023c759b53.png` to `/backups/car-erp/cars/1ff7f0ca-4ee5-4cfe-ad1a-93023c759b53/v2.gzip`

You can see that the version is used as file name.
The general rule is that the file should not be deleted until it the user has deleted it for few days.
In ideal situation only the latest version is stored as backup.

_Note: You should use a separate database to keep backup information as it is exposed to internet, this approach reduces
the need to have a separate API server._
