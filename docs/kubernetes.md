# Kubernetes
This guide sets up a local Kubernetes development environment using Docker Desktop, kubectl, and helm. It also includes steps to enable the Kubernetes Dashboard for visual cluster management.

## Development environment
### Activate Kubernetes in Docker Desktop
* Open Docker Desktop.
* Go to Settings > Kubernetes.
* Enable the checkbox: Enable Kubernetes.
* Wait for Kubernetes to start (you'll see a green light or similar status when ready).

### Install Required Tools
Make sure you have the following installed:
* [kubectl](https://kubernetes.io/docs/tasks/tools/) – Kubernetes command-line tool. 
* [helm](https://helm.sh/docs/intro/install/) – Kubernetes package manager.

### Kubernetes Dashboard
Enable kubernetes dashboard by this command:
```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.2.0/aio/deploy/recommended.yaml
```

*TODO: Create admin-user*

Create a login token to get access of the dashboard:
```bash
kubectl -n kubernetes-dashboard create token admin-user
```