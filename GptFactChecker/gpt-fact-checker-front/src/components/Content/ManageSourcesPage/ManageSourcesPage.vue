<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { useSourcesStore } from "@/stores/sources";
import { storeToRefs } from "pinia";
import { notification } from "ant-design-vue";

const sourcesStore = useSourcesStore();
const { filteredSources, loadingSources } = storeToRefs(sourcesStore);

const initLoading = ref(true);

const fetchSources = async () => {
	try {
		await sourcesStore.getSourcesAsync();
	} catch (error) {
		console.error(error);
	} finally {
	}
};

onMounted(() => {
	fetchSources();
	initLoading.value = false;
});
//------ Navigation

const router = useRouter();

function navigateToDetails(sourceId: string): void {
	router.push(`/source/${sourceId}`);
};

//------ Manage sources

async function removeSource(sourceId: string) {
	try {
		const deleteSourceResult = await sourcesStore.deleteSourceAsync(sourceId);

		if (!deleteSourceResult) {
			openNotificationWithIcon(
				"error",
				"Failed to remove source",
				"Couldn't remove source."
			);
		}

		openNotificationWithIcon("success", "Source removed", "Removed source.");

		await fetchSources();
	} catch (error) {
		console.error(error);
		openNotificationWithIcon(
			"error",
			"Failed to remove source",
			"Couldn't remove source."
		);
	} finally {
	}
}

type NotificationType = "success" | "error" | "info" | "warning";

const openNotificationWithIcon = (
	notificationType: NotificationType,
	title: string,
	message: string
) => {
	if (notificationType in notification) {
		notification[notificationType]({
			message: title,
			description: message,
		});
	} else {
		console.log("Failed to create notification with type: " + notificationType);
	}
};
</script>

<template>
	<div>
		<a-list
		:loading="loadingSources || initLoading"
		item-layout="horizontal"
		:data-source="filteredSources"
		>
		<template #renderItem="{ item: source }">
			<a-list-item>
				<template #actions>
					<a
					key="more"
						@click="navigateToDetails(source.id)"
						>Details</a
					>
					<a key="edit">Edit</a>
					<a
					key="delete"
						@click="removeSource(source.id)"
						>Remove</a
					>
				</template>
				<a-skeleton
				:title="false"
					:loading="!!source.loading"
					>
					<a-list-item-meta :description="source.description">
						<template #title>
							<div>
								<h3 class="source-title">{{ source.name }}</h3>
							</div>
						</template>
					</a-list-item-meta>
					<div class="list-item-content">
						<a
						:href="source.sourceUrl"
						target="_blank"
						>Link to Source</a
						>
					</div>
				</a-skeleton>
			</a-list-item>
		</template>
	</a-list>
</div>
</template>

<style scoped>
.source-title {
	margin-top: 0;
	margin-bottom: 0;
}
</style>
