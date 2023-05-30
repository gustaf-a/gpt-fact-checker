import { ref } from "vue";
import { defineStore } from "pinia";
import axios from "axios";
import Source from "@/model/Source";
import { Keys } from "./constants";
import { ErrorMessages } from "@/utils/errors";

const BaseUrl = "http://localhost:5067";

export const useSourcesStore = defineStore(Keys.SOURCES, () => {
	const sources = ref<Source[]>([]);

	const errorMessage = ref("");
	const loadingSources = ref(false);

	async function getSourcesAsync(): Promise<void> {
		if (sources.value.length > 0) return;

		try {
			loadingSources.value = true;

			const response = await axios.get(`${BaseUrl}/api/sources`);

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

		console.log("Trying to fetch source with id ${sourceId} from backend.");

		try {
			loadingSources.value = true;

			const response = await axios.get(
				`${BaseUrl}/api/sources/id?id=${sourceId}`
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
		try {
			const response = await axios.delete(
				`${BaseUrl}/api/sources/id?id=${sourceId}`
			);

			if (response.status !== 200) {
				errorMessage.value = ErrorMessages.DELETE_RESOURCE_ERROR;
				return false;
			}

			sources.value = sources.value.filter(source => source.id !== sourceId);

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
		if (!source) {
			console.log("Can't create null source.");
			return false;
		}

		try {
			loadingSources.value = true;

			console.log("Sending to backend");
			const response = await axios.post(`${BaseUrl}/api/sources`, source);

			if (response.status !== 200) {
				errorMessage.value = ErrorMessages.CREATE_RESOURCE_ERROR;
				console.log(ErrorMessages.CREATE_RESOURCE_ERROR);
				console.log(source);
				return false;
			}

			console.log("Adding to current sources");
			sources.value.push(source);
			return true;
		} catch (error) {
			errorMessage.value = `${ErrorMessages.CREATE_RESOURCE_ERROR}: ${
				error instanceof Error ? error.message : String(error)
			}`;
			console.log(error);
			console.log(source);
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
		deleteSourceAsync
	};
});
