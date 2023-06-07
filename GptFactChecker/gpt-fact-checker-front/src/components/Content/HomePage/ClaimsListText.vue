<script setup lang="ts">
import { onMounted, ref } from "vue";
import ClaimWithSource from "@/model/ClaimWithSource";
import ListTextItem from "./ListTextItem.vue";

interface Props {
	maxLength: number;
	claimsWithSource: ClaimWithSource[];
}

const { maxLength, claimsWithSource } = defineProps<Props>();

const claims = ref<ClaimWithSource[]>([]);

onMounted(() => {
	claims.value = claimsWithSource;
});
</script>

<template>
	<div class="object-list-container">
		<div class="headline">
			<slot></slot>
		</div>
		<a-list
			item-layout="horizontal"
			:data-source="claims"
		>
			<template #renderItem="{ item }">
				<ListTextItem
					:navigate-to-path="`/source/${item.source.id}`"
					:item-name="item.claimSummarized"
					:item-context="item.source.sourceContext"
					item-description=""
					:item-person="item.source.sourcePerson"
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
}

.headline {
	margin-top: 1vh;
	opacity: 0.7;
	margin-bottom: 0;
}

.divider {
	margin-top: 0;
	margin-bottom: 0;
}
</style>
