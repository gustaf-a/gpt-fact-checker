<script setup lang="ts">
import SourceObject from "@/model/Source";
import router from "@/router";
import IncorrectClaims from "./ClaimsStatistics.vue"

interface Props {
	source: SourceObject;
}

const { source } = defineProps<Props>();

const navigateToSource = () => {
	router.push(`/source/${source.id}`);
};

</script>

<template>
	<div
		class="source-card"
		@click="navigateToSource"
	>
		<a-card :title="source.name">
			<!-- Fact check status: Needs fact checking,  -->
			
			<IncorrectClaims :source="source" />
			<p>{{ source.sourceCreatedDate }}</p>
		</a-card>
	</div>
</template>

<style scoped>
.source-card {
	margin: 10px;
	width: 35vw;
	border: 1px solid #f0f0f0;
	border-radius: 2px;
	box-shadow: 0 2px 8px #f0f0f0;
}
</style>
