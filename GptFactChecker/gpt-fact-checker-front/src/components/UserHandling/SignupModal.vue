<script setup lang="ts">
import { ref, reactive } from "vue";
import { useUserStore } from "@/stores/users";
import { storeToRefs } from "pinia";
import { NotificationSuccess } from "@/utils/notifications";
import UserSignup from "@/model/UserSignup";

const userStore = useUserStore();
const { user, errorMessage, loadingUser } = storeToRefs(userStore);

const modalVisible = ref(false);

const showModal = () => {
	modalVisible.value = true;
};

const userSignup = reactive<UserSignup>({
	email: "",
	password: "",
	userName: "",
	name: ""
});

const handleOk = async () => {
	await userStore.handleSignup({
		password: userSignup.password.trim(),
		email: userSignup.email.trim(),
		userName: userSignup.userName.trim(),
		name: userSignup.name.trim()
	});

	if (errorMessage.value === "") {
		NotificationSuccess("Signup Success!", "Signup successful! Now please login");

		setTimeout(() => {
			modalVisible.value = false;
			clearInput();
		}, 200);
	}
};

const handleCancel = (e:any) => {
	clearInput();

	modalVisible.value = false;
};

function clearInput() {
	errorMessage.value = "";

	userSignup.email = "";
	userSignup.password = "";
	userSignup.userName = "";
	userSignup.name = "";
}
</script>

<template>
	<div>
		<a-button
			class="button"
			@click="showModal"
			>Signup</a-button
		>
		<a-modal
			:visible="modalVisible"
			@ok="handleOk"
			@cancel="handleCancel"
		>
			<template #footer>
				<a-button
					key="back"
					@click="handleCancel"
					>Cancel</a-button
				>
				<a-button
					key="submit"
					:disabled="loadingUser"
					type="primary"
					:loading="loadingUser"
					@click="handleOk"
					>Signup</a-button
				>
			</template>
			<div
				v-if="!loadingUser"
				class="input-container"
			>
				<a-input
					class="input"
					v-model:value="userSignup.name"
					placeholder="Name"
				/>
				<a-input
					class="input"
					v-model:value="userSignup.userName"
					placeholder="UserName"
				/>
				<a-input
					class="input"
					v-model:value="userSignup.email"
					placeholder="Email"
				/>
				<a-input
					class="input"
					v-model:value="userSignup.password"
					placeholder="Password"
					type="password"
					@keyup.enter="handleOk"
				/>
				<a-typography-text
					type="danger"
					v-if="errorMessage"
					>{{ errorMessage }}</a-typography-text
				>
			</div>
			<div
				v-else
				class="spinner"
			>
				<a-spin></a-spin>
			</div>
		</a-modal>
	</div>
</template>

<style scoped>
.input-container{
    margin-top: 3vh;
}

.input{
    margin-top: 1vh
}
</style>
