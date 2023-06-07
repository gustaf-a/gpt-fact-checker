<script setup lang="ts">
import { onMounted, ref } from "vue";
import SourceObject from "@/model/Source";
import ListTextItem from "./ListTextItem.vue";

interface Props {
	maxLength: number;
	sourcesInList: SourceObject[];
}

const { maxLength, sourcesInList } = defineProps<Props>();

const sources = ref<SourceObject[]>([]);

onMounted(() => {
	sources.value = sourcesInList;
});
</script>

<template>
	<div class="object-list-container">
		<div class="sources-headline">
			<slot></slot>
		</div>
		<a-list
			item-layout="horizontal"
			:data-source="sources"
		>
			<template #renderItem="{ item }">
				<ListTextItem
					:navigate-to-path="`/source/${item.id}`"
					:item-name="item.name"
					:item-context="item.sourceContext"
					:item-description="item.description"
					:item-person="item.sourcePerson"
					:includeDescription="false"
					:max-length="maxLength"
				></ListTextItem>
			</template>
		</a-list>
	</div>
</template>

<style scoped>
.object-list-container{
	max-width: 40vw;
	cursor: pointer;
}

.sources-headline {
	margin-top: 1vh;
	opacity: 0.7;
	margin-bottom: 0;
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
</style>
