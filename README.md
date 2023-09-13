# edc23-radix-workshop-2
EDC23 Radix workshop example to use [Radix batches and jobs](https://radix.equinor.com/guides/jobs/)
# EDC 2023 conference, Radix application example on dotnet core

### Prepare the app
* Create the new application, using this repository template
* Install [Docker](https://docs.docker.com/get-docker/)
* Install environment to run dotnet core
  * Option 1: install [dotnet core](https://docs.microsoft.com/en-us/dotnet/core/install/)
  * Option 2: install [Visual Studio](https://visualstudio.microsoft.com/downloads/) and [Dev Container extension](https://code.visualstudio.com/docs/devcontainers/containers) for VS Code and run the application in the dev container
* Change an application name in the `radixconfig.yaml`, so it is unique within the Radix cluster
```yaml
metadata:
  name: your-app-name
```
* [Register an Radix application](https://radix.equinor.com/start/registering-app/) in the Radix console
* [Build and deploy](https://radix.equinor.com/guides/build-and-deploy/) the application in the Radix console, `Pipeline Jobs` page. Navigate to the `dev` environment to see deployed components and its replicas.
* Commit and push the changes to the GitHub - Radix should run a new pipeline job to build and deploy the application components
* Change the code - e.g. add a field to the model
* Commit and push the changes to the GitHub - Radix should run the pipeline to build and deploy components with their changes.
* Add code editor integration with `radixconfig.yaml`: [Visual Studio Code](https://radix.equinor.com/references/reference-code-editor-integration/#visual-studio-code) or [JetBrains IDE](https://radix.equinor.com/references/reference-code-editor-integration/#jetbrains-ides)

### Generate a client for JobManager API
A [client to the JobManager API](https://radix.equinor.com/guides/jobs/openapi-swagger.html) is generated from the [OpenAPI specification file](https://raw.githubusercontent.com/equinor/radix-public-site/main/public-site/docs/src/guides/jobs/swagger.json). The client is used to submit jobs to the JobManager API with regular code objects and methods, instead of using a HTTP client and json data.
* Install [OpenAPI generator](https://github.com/OpenAPITools/openapi-generator#1---installation)
* Run the command from the solution folder
```bash
openapi-generator generate -g csharp  -i https://raw.githubusercontent.com/equinor/radix-public-site/main/public-site/docs/src/guides/jobs/swagger.json  -o frontend/batch-client
```
* Add the generated client to the frontend project as a reference

### Run the web-app locally
* Run the application locally and open in a browser a link [http://localhost:8070](http://localhost:8000).
  ```bash
  cd path-to-web-app-project-folder
  dotnet watch run
  ```

### Run the task locally
* Run the application locally with an env var `PAYLOAD_FILE=path-to-payload-file`
  ```bash
  cd path-to-task-project-folder
  PAYLOAD_FILE=path-to-payload-file dotnet run
  ```

### Run the web-app application in the Docker container with docker
* Navigate to the folder `frontend`
* Build the docker image `docker build . -t web-app`
* Run the container from the image `docker run -it -p 8070:8070 -p 8060:8060 -e ASPNETCORE_URLS=http://*:8070;http://*:8060 web-app`
* To test web-app, open in a browser a link [http://localhost:8070](http://localhost:8070/).
* To test BatchStatus endpoint, send http request: `curl -X POST localhost:8060/BatchStatuses -d '{"batchName":"","name": "batch-compute-20230220101417-idwsxnc", "created": "2023-02-20T10:14"}' -H "Content-Type: application/json" --verbose`
* Hit Ctrl+C to shutdown the running web-app

### Run the evaluate-value application in the Docker container with docker
* Navigate to the folder `jobs/task`
* Build the docker image `docker build . -t task`
* Run the container from the image `docker run -it -v ./input:/app/input task`
* The container runs the program, an optional input can be passed via payload in the `payload` file in the `/app/input/payload` folder
* For troubleshooting, run the container with `docker run -it -v ./input:/app/input --entrypoint bash task`, which starts a bash shell within the running container, allowing explore container's file system

### Run the web-app application in the Docker container with docker compose
* Navigate to the folder `frontend`
* Run `docker-compose up` (or `docker-compose up --build` to rebuild existing container layers).
* Open in a browser a link [http://localhost:8070](http://localhost:8070/).
* Hit Ctrl+C or run `docker-compose down` to shutdown the running containers.

### Configurations
* `radixconfig.yaml` - Radix configuration file for basic functionality
* `radixconfig2.yaml` - Radix configuration file to demonstrate notifications webhook
* `radixconfig3.yaml` - Component `frontend/api` displays body of a request, it can be used to explore notification webhook requests.
* `radixconfig3.yaml` - Component `frontend/api` displays body of a request, it can be used to explore notification webhook requests.