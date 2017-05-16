# Fuze
The vision is to give a given pipeline the tool to automate deployment into a kubernetes cluster, which in turn allows end to end testing for an app without having to worry about running commands directly on the kubernetes cluster. 
An advantage of this approach is that the .yml files are kept, which serves as documentation to how the deployment happened. 

## Using Fuze
Simply make a 'k8settings.yml' file in the root of the project and run the project. 
It needs the following parameters: 
```
---
configs:
 - name: the-name-of-your-environment
   appName: the-name-of-your-application
   domain: 
    - 'the-domain-this-will-respond-to'
    - 'another-domain-this-will-respond-to'
    - 'and-so-on....'
   image: 'nginx:latest-prod'
   env:
     - the-key-you-want: the-value-you-want
   secret:
     - the-key-you-want: the-value-you-want-in-base64
``` 
and can be extended as follows
```
---
configs:
 - name: prod
   appName: foo
   domain: 
    - 'foo.bar.com'
    - 'foo.bar.com'
   image: 'nginx:latest-prod'
   env:
     - key1: value
   secret:
     - key1: value
 - name: train
   appName: foo
   domain: 
    - 'foo.bar.com'
    - 'foo.bar.com'
   image: 'nginx:latest-train'
   env:
     - key1: value
   secret:
     - key1: value          
 - name: test
   appName: foo
   domain: 
    - 'foo.bar.com'
    - 'foo.bar.com'
   image: 'nginx:latest-test'
   env:
     - key1: value
   secret:
     - key1: value
 - name: dev
   appName: foo
   domain: 
    - 'foo.bar.com'
    - 'foo.bar.com'
   image: 'nginx:latest-dev'
   env:
     - key1: value
   secret:
     - key1: value
```

### Dependencies
It depends on dotnet core libraries.
