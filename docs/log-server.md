# Log Server
> [!WARNING]  
> After deployment, change the password of the default user.

## Setup server
1. Create a new API key for the API server. Add this key to the Kubernetes dashboard in the secret `apikeyapi`.
2. Configure a retention policy 
   * Go to Menu → Data → Storage → Retention Policy and set the desired retention rules.