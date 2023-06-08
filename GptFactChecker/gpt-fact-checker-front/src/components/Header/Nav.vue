<script setup lang="ts">
import Container from "../Container.vue";
import { ref } from "vue";
import { useUserStore } from "@/stores/users";
import { storeToRefs } from "pinia";
import LoginModal from "../UserHandling/LoginModal.vue";
import SignupModal from "../UserHandling/SignupModal.vue";

const userStore = useUserStore();
const { user } = storeToRefs(userStore);

const initLoading = ref(true);

const handleLogout = async () => {
	await userStore.handleLogout();
}

</script>

<template>
	<a-layout-header>
		<Container>
			<div class="container-navigation">
				<div class="navigation-left">
					<RouterLink
						class="logo"
						to="/"
						>FactCheckMe</RouterLink
					>

					<RouterLink
						class="nav-item nav-button"
						to="/sources"
						>FactChecks</RouterLink
					>
					<!-- 
					<RouterLink
						class="nav-item nav-button"
						to="/resources"
						>Resources</RouterLink
					> -->

					<RouterLink
						class="nav-item nav-button"
						to="/about"
						>About</RouterLink
					>

					<!-- TODO Insert items to Top List etc  -->

					<!-- <a-input-search
						class="nav-item navigation-search"
						placeholder="Search.."
					/> -->
				</div>
				<div
					v-if="user"
					class="navigation-right"
				>
					<RouterLink
						class="nav-item nav-button"
						to="/managesources"
						>Manage Sources</RouterLink
					>
					<!-- <a-button class="nav-item">MyPages</a-button> -->
					<a-button class="nav-item" @click="handleLogout">Logout</a-button>
				</div>
				<div
					v-else
					class="navigation-right"
				>
					<LoginModal class="nav-item discrete" />
					<SignupModal class="nav-item discrete" />
				</div>
			</div>
		</Container>
	</a-layout-header>
</template>

<style scoped>
.ant-layout-header {
	color: #fff;
	background: #576f72;
}

.container-navigation {
	height: 8vh;
	display: flex;
	align-items: center;
	justify-content: space-between;
}

.navigation-left {
	display: flex;
	align-items: center;
}

.logo {
	font-size: x-large;
	color: white;
	margin-right: 2vw;
}

.nav-item {
	margin-left: 1vw;
	color: black;
}

.nav-button {
	color: white;
	font-size: larger;
}

.discrete{
	opacity: 0.9;
}

.navigation-right {
	display: flex;
	align-items: center;
}
</style>
