pipeline {
    agent any
    environment {
        REPO_FE = "inout/inout-frontend"
        REPO_API = "inout/inout-api"
        PRIVATE_REPO_FE = "${PRIVATE_DOCKER_REGISTRY}/${REPO_FE}"
        PRIVATE_REPO_API = "${PRIVATE_DOCKER_REGISTRY}/${REPO_API}"
        TAG = "j-${env.BUILD_NUMBER}"
    }
    stages {
        stage('Docker build - API') {
            steps {
                git branch: 'master',
                    credentialsId: 'JENKINS-AZUREDEVOPS',
                    url: 'git@ssh.dev.azure.com:v3/LivingSkySchoolDivision/InOutBoard/InOutBoard'

                dir("Backend") {
                    sh "docker build -t ${PRIVATE_REPO_API}:latest -t ${PRIVATE_REPO_API}:${TAG} ."
                }
            }
        }
        stage('Docker build - Frontend') {
            steps {
                git branch: 'master',
                    credentialsId: 'JENKINS-AZUREDEVOPS',
                    url: 'git@ssh.dev.azure.com:v3/LivingSkySchoolDivision/InOutBoard/InOutBoard'

                dir("Frontend") {
                    sh "docker build -t ${PRIVATE_REPO_FE}:latest -t ${PRIVATE_REPO_FE}:${TAG} ."
                }
            }
        }
        stage('Docker push') {
            steps {
                sh "docker push ${PRIVATE_REPO_FE}:${TAG}"
                sh "docker push ${PRIVATE_REPO_FE}:latest"
                sh "docker push ${PRIVATE_REPO_API}:${TAG}"
                sh "docker push ${PRIVATE_REPO_API}:latest"
            }
        }
    }
    post {
        always {
            deleteDir()
        }
    }
}