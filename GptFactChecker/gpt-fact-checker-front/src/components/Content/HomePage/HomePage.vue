<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useSourcesStore } from "@/stores/sources";
import SourceObject from "@/model/Source";
import Container from "@/components/Container.vue";
import SourcesList from "./SourcesListText.vue";
import ClaimsList from "./ClaimsListText.vue"
import type ClaimWithSource from "@/model/ClaimWithSource";

const sourcesStore = useSourcesStore();
const sources = ref<SourceObject[]>([]);

const loadingSources = ref<boolean>(true);

const sourcesRecent = ref<SourceObject[]>([]);
const loadingSourcesRecent = ref<boolean>(true);

const sourcesMostViewed = ref<SourceObject[]>([]);
const loadingSourcesMostViewed = ref<boolean>(true);

const claimsMostEngaging = ref<ClaimWithSource[]>([]);
const loadingClaimsEngaging = ref<boolean>(true);

const fetchSources = async () => {
	loadingSources.value = true;

	try {
		await sourcesStore.getSourcesAsync();
		sources.value = sourcesStore.sources;
	} catch (error) {
		console.error(error);
	} finally {
		loadingSources.value = false;
	}
};

onMounted(async () => {
	await fetchSources();

	assignRecentSources();
	assignMostViewedSources();
    assignMostEngagingClaims();
});

function assignRecentSources() {
	sourcesRecent.value = sources.value;
    loadingSourcesRecent.value = false;
}

function assignMostViewedSources() {
	sourcesMostViewed.value = sources.value;
    loadingSourcesMostViewed.value = false;
}

function assignMostEngagingClaims() {
    if(!sources.value[0].claims ||sources.value[0].claims.length < 2) return;
    if(!sources.value[1].claims ||sources.value[1].claims.length < 2) return;
    
    if (sources.value[0].claims.length > 0 && sources.value[1].claims.length > 0) {
        const claim1 = sources.value[0].claims[0]; // extract 1st claim from sources.value[0]
        const claim2 = sources.value[1].claims[0]; // extract 1st claim from sources.value[1]

        //convert to claimWithSource objects
        const claimWithSource1: ClaimWithSource = {
            ...claim1,
            source: sources.value[0]
        };

        const claimWithSource2: ClaimWithSource = {
            ...claim2,
            source: sources.value[1]
        };

        claimsMostEngaging.value = [claimWithSource1, claimWithSource2];
    }

    loadingClaimsEngaging.value = false;
}
</script>

<template>
	<Container>
        <div class="list-container">
            <SourcesList
			v-if="!loadingSourcesMostViewed"
			:sources-in-list="sourcesMostViewed"
			:max-length="80"
			><h2 class="sources-headline">Most viewed sources</h2></SourcesList
            >
        </div>
        <div class="list-container">
            <SourcesList
			v-if="!loadingSourcesRecent"
			:sources-in-list="sourcesRecent"
			:max-length="80"
			><h2 class="sources-headline">Recently added sources</h2></SourcesList
            >
        </div>
        
        <div class="list-container">
            <ClaimsList
            v-if="!loadingClaimsEngaging"
            :claims-with-source="claimsMostEngaging"
            :max-length="200"
            ><h2 class="sources-headline">Most engaging claims</h2></ClaimsList
            >
        </div>
	</Container>
</template>

<style scoped>
.list-container{
    background-color: #f5f5f5; 
    border: 1px solid #dcdcdc; 
    box-shadow: 0px 4px 8px 0px rgba(0, 0, 0, 0.2); 
    padding: 20px;
    border-radius: 5px;
    max-width: 40vw;
    margin: 2vh 1vh;
}

.sources-headline {
	margin-top: 0vh;
	opacity: 1;
	margin-bottom: 0;
    font-size: 1.3em;
}

.divider {
	margin-top: 0;
	margin-bottom: 0;
}

.sources-container {
	display: flex;
	flex-wrap: wrap;
	align-items: center;
	justify-content: center;
}

.source {
	cursor: pointer;
}
</style>
