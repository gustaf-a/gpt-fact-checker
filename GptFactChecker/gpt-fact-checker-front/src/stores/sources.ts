import { ref } from "vue";
import { defineStore } from "pinia";
import axios from "axios";
import { Keys, Urls } from "./constants";
import { ErrorMessages } from "@/utils/errors";
import Source from "@/model/Source";
import { useUserStore } from "@/stores/users";

export const useSourcesStore = defineStore(Keys.SOURCES, () => {
	const userStore = useUserStore();
	const { userHasRole, Roles } = userStore;

	const sources = ref<Source[]>([]);

	const errorMessage = ref("");
	const loadingSources = ref(false);

	async function getSourcesAsync(): Promise<void> {
		try {
			loadingSources.value = true;

			const response = await axios.get(`${Urls.BASE_URL}/api/sources`);

			if (response.status !== 200) {
				errorMessage.value = ErrorMessages.DATA_FETCH_ERROR;
				return;
			}

			sources.value = response.data;
		} catch (error) {
			errorMessage.value = `${ErrorMessages.DATA_FETCH_ERROR}: ${
				error instanceof Error ? error.message : String(error)
			}`;
		} finally {
			loadingSources.value = false;
		}
	}

	async function getSourceByIdAsync(sourceId: string): Promise<Source | null> {
		const localSource = sources.value.find((s) => s.id === sourceId);
		if (localSource) return localSource;

		let foundSource: Source | null = null;

		try {
			loadingSources.value = true;

			const response = await axios.get(
				`${Urls.BASE_URL}/api/sources/id?id=${sourceId}`
			);

			if (response.status !== 200) {
				errorMessage.value = "Error fetching data";
			} else {
				foundSource = response.data;
			}
		} catch (error) {
			if (error instanceof Error) {
				errorMessage.value = "Error fetching data: " + error.message;
			} else {
				errorMessage.value = "Error fetching data: " + String(error);
			}
		} finally {
			loadingSources.value = false;
		}

		return foundSource;
	}

	async function deleteSourceAsync(sourceId: string): Promise<boolean> {
		if (!userHasRole(Roles.DELETESOURCES)) return false;
		
		try {
			const response = await axios.delete(
				`${Urls.BASE_URL}/api/sources/id?id=${sourceId}`
			);

			if (response.status !== 200) {
				errorMessage.value = ErrorMessages.DELETE_RESOURCE_ERROR;
				return false;
			}

			sources.value = sources.value.filter((source) => source.id !== sourceId);

			return true;
		} catch (error) {
			errorMessage.value = `${ErrorMessages.DELETE_RESOURCE_ERROR}: ${
				error instanceof Error ? error.message : String(error)
			}`;
			console.log(error);
			console.log("Failed to delete source: " + sourceId);
			return false;
		} finally {
		}
	}

	async function addSourceAsync(source: Source): Promise<boolean> {
		if (!userHasRole(Roles.ADDSOURCES)) return false;
		
		if (!source) {
			console.log("Can't create null source.");
			return false;
		}

		try {
			loadingSources.value = true;

			const response = await axios.post(`${Urls.BASE_URL}/api/sources`, source);

			if (response.status !== 200) {
				errorMessage.value = ErrorMessages.CREATE_RESOURCE_ERROR;
				console.log(ErrorMessages.CREATE_RESOURCE_ERROR, source);
				return false;
			}

			sources.value.push(source);
			return true;
		} catch (error) {
			errorMessage.value = `${ErrorMessages.CREATE_RESOURCE_ERROR}: ${
				error instanceof Error ? error.message : String(error)
			}`;
			console.log(error, source);
			return false;
		} finally {
			loadingSources.value = false;
		}
	}

	return {
		sources,
		errorMessage,
		loadingSources,
		getSourcesAsync,
		getSourceByIdAsync,
		addSourceAsync,
		deleteSourceAsync,
	};
});
