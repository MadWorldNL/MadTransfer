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
Enable the Kubernetes Dashboard by running:
```bash
kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v2.2.0/aio/deploy/recommended.yaml
```

Apply the admin-user configuration (found at [dashboard-admin-user.yaml](../deployment/development/dashboard-admin-user.yml)) to create a ServiceAccount with admin privileges:
```bash
kubectl apply -f dashboard-admin-user.yml
```

Generate a login token for accessing the dashboard and copy to your clipboard:
```bash
kubectl -n kubernetes-dashboard create token admin-user --duration=12h
```
First, find the name of the dashboard pod:
```bash
kubectl get pods --all-namespaces
```
You can inspect the dashboard pod details using:
```bash
kubectl describe pod kubernetes-xxxxxxxxxx-xxxxx -n kubernetes-dashboard
```
This is useful for debugging or verifying that the pod is running correctly.

Then forward port 8443 to your local machine (replace the pod name as needed):
```bash
kubectl port-forward kubernetes-dashboard-xxxxxxxxxx-xxxxx 8443:8443 -n kubernetes-dashboard
```

Now open your browser and visit: https://localhost:8443/. Ignore any browser warnings about self-signed certificates and proceed.

## Production
### Install on production
#### Step 1: Install MicroK8s
Execute this install command:
```shell
sudo snap install microk8s --classic
sudo microk8s status --wait-ready
```

#### Step 2: Enable services
Required:
```shell
sudo microk8s enable dns
sudo microk8s enable helm
sudo microk8s enable dashboard
sudo microk8s enable ingress
sudo microk8s enable cert-manager
sudo microk8s enable hostpath-storage
```

Optional:
```shell
sudo microk8s enable metrics-server
sudo microk8s enable prometheus
```

### Usage on production
#### Step 1 - Download source code
```shell
git clone https://github.com/MadWorldNL/MadTransfer
```

#### Step 2 - Install Cloud Native PG Operator
You can install the latest operator manifest for this minor release as follows:
```shell
kubectl apply --server-side -f \
  https://raw.githubusercontent.com/cloudnative-pg/cloudnative-pg/release-1.26/releases/cnpg-1.26.1.yaml
```

#### Step 2 - Install Cluster
Navigate to the folder `deployment/MadTransfer` and execute this command:
```shell
microk8s helm install -f environments/values-production.yaml mad-transfer .
```

#### Step 3 - Status of Cluster
Convenient tools for debugging Kubernetes:
```shell
microk8s dashboard-proxy --address 0.0.0.0
```

### Reference
[MicroK8s install guide](https://microk8s.io/)