<script setup lang="ts">
import router from "@/router";

interface Props {
	includeDescription: boolean;
	maxLength: number;
	navigateToPath: string;
	itemName: string;
	itemPerson: string;
	itemContext: string;
	itemDescription: string;
}

const { itemName, itemPerson, itemContext, itemDescription, includeDescription, maxLength, navigateToPath } = defineProps<Props>();

const navigateTo = () => {
	router.push(navigateToPath);
};

function getTitle() {
	let proposedTitle = itemName + " - " + itemPerson + ", " + itemContext

	if(proposedTitle.length <= maxLength) return proposedTitle;
	
	proposedTitle = itemName + " - " + itemContext
	
	if(proposedTitle.length <= maxLength) return proposedTitle;
	
	return itemName.slice(0, maxLength);
}

function getDescription() {
	if (includeDescription) {
		return itemDescription.slice(0, maxLength).trim() + "..";
	}

	return undefined;
}
</script>

<template>
	<a-list-item
		class="list-item"
		@click="navigateTo"
	>
		<a-list-item-meta :description="getDescription()">
			<template #title>
				<a @click="navigateTo">{{ getTitle() }}</a>
			</template>
		</a-list-item-meta>
	</a-list-item>
</template>

<style scoped>
.list-item {
	cursor: pointer;
	margin: 0;
}
</style>
