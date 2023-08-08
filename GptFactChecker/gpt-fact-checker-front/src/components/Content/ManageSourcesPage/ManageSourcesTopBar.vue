<script setup lang="ts">
import AddNewSourceButton from "@/components/Content/ManageSourcesPage/AddNewSourceButton.vue";
import AddNewSourceFromUrlButton from "@/components/Content/ManageSourcesPage/AddNewSourceFromUrlButton.vue";
import { useSourcesStore } from "@/stores/sources";
import { useUserStore } from "@/stores/users";
import { ref, watch } from "vue";
import FilterOptions from "@/model/FilterOptions";

const { applyFilters } = useSourcesStore();

const userStore = useUserStore();
const { userHasRole, Roles } = userStore;

const searchText = ref<string>("");

watch(searchText, () => {
	applyFilters(new FilterOptions(searchText.value.toLowerCase()));
});
</script>

<template>
	<div class="manage-source-top-bar">
		<div>
			<a-input-search
				v-model:value.trim="searchText"
				class="nav-item navigation-search"
				placeholder="Search.."
			/>
		</div>
		<div
			class="new-source-container"
			v-if="userHasRole(Roles.ADDSOURCES)"
		>
			<!-- <AddNewSourceButton/> -->
			<AddNewSourceFromUrlButton/>
		</div>
	</div>
</template>

<style scoped>
.manage-source-top-bar {
	margin-top: 2vh;
}
</style>
