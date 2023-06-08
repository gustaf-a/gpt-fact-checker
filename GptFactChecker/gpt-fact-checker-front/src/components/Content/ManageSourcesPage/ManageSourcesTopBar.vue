<script setup lang="ts">
import AddNewSourceDrawer from "@/components/Content/ManageSourcesPage/AddNewSourceDrawer.vue";
import { useSourcesStore } from "@/stores/sources";
import { useUserStore } from "@/stores/users";
import { ref, watch } from "vue";
import SourcesFilterOptions from "@/model/SourcesFilterOptions"

const { applyFilters } = useSourcesStore();

const userStore = useUserStore();
const { userHasRole, Roles } = userStore;

const searchText = ref<string>('');

watch(searchText, () => {
    applyFilters(new SourcesFilterOptions(searchText.value.toLowerCase()));
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
			<AddNewSourceDrawer />
		</div>
	</div>
</template>

<style scoped>
.manage-source-top-bar {
	margin-top: 2vh;
}
</style>
