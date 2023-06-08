<script setup lang="ts">
import { ref, reactive } from "vue";
import { useUserStore } from "@/stores/users";
import { storeToRefs } from "pinia";
import UserLogin from "@/model/UserLogin";
import { NotificationSuccess } from "@/utils/notifications";

const userStore = useUserStore();
const { user, errorMessage, loadingUser } = storeToRefs(userStore);

const modalVisible = ref(false);

const showModal = () => {
	modalVisible.value = true;
};

const userLogin = reactive<UserLogin>({
	email: "",
	password: "",
});

const handleOk = async () => {
	await userStore.handleLogin({
		password: userLogin.password.trim(),
		email: userLogin.email.trim(),
	});

	if (user.value) {
		NotificationSuccess("Login Success!", "Login successful!");

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

	userLogin.email = "";
	userLogin.password = "";
}
</script>

<template>
	<div>
		<a-button
			class="button"
			@click="showModal"
			>Login</a-button
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
					>Login</a-button
				>
			</template>
			<div
				v-if="!loadingUser"
				class="input-container"
			>
				<a-input
					class="input"
					v-model:value="userLogin.email"
					placeholder="Email"
				/>
				<a-input
					class="input"
					v-model:value="userLogin.password"
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
