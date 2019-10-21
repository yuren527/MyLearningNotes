# Get Mesh Data From Static Mesh #
```C++
#include "MyGameModeBase.h"
#include "Engine/StaticMesh.h"
#include <vector>

using namespace std;

void AMyGameModeBase::PrintMeshData(UStaticMesh* mesh) {
	FPositionVertexBuffer* positionVertexBuffer = &(mesh->RenderData->LODResources[0].VertexBuffers.PositionVertexBuffer);
	vector<FVector> vertices;
	int vertexCount = positionVertexBuffer->GetNumVertices();
	for (int i = 0; i < vertexCount; i++) {
		vertices.push_back(positionVertexBuffer->VertexPosition(i));
	}
	
	FRawStaticIndexBuffer* indexBuffer = &(mesh->RenderData->LODResources[0].IndexBuffer);
	vector<int> triangles;
	int triangleCount = indexBuffer->GetNumIndices();
	for (int i = 0; i < triangleCount; i++) {
		triangles.push_back(indexBuffer->GetIndex(i));
	}
	
	UE_LOG(LogTemp, Warning, TEXT("Vertex Count: %d"), vertexCount);
	UE_LOG(LogTemp, Warning, TEXT("Triangle Count: %d"), triangleCount);

	for (FVector v : vertices) {
		UE_LOG(LogTemp, Warning, TEXT("%f, %f, %f"),v.X, v.Y, v.Z);
	}

	for (int i : triangles) {
		UE_LOG(LogTemp, Warning, TEXT("%d"), i);
	}
}
```
