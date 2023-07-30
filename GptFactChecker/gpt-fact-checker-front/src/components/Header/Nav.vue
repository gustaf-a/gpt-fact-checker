<script setup lang="ts">
import Container from "../Container.vue";
import { ref } from "vue";
import LoginModal from "../UserHandling/LoginModal.vue";
import SignupModal from "../UserHandling/SignupModal.vue";
import { useUserStore } from "@/stores/users";
import { storeToRefs } from "pinia";
import useColorScheme from '@/stores/colorScheme'

//Ensure CSS Variables are created
useColorScheme()

const userStore = useUserStore();
const { userHasRole, Roles } = userStore;
const { user } = storeToRefs(userStore);

const handleLogout = async () => {
	await userStore.handleLogout();
};
</script>

<template>
	<a-layout-header>
		<Container>
			<div class="container-navigation">
				<div class="navigation-left">
					<RouterLink
						class="logo"
						to="/"
						>FactFriend</RouterLink
					>

					<RouterLink
						class="nav-item nav-button"
						to="/sources"
						>Sources</RouterLink
					>

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
						v-if="userHasRole(Roles.EDITREFERENCES)"
						class="nav-item nav-button"
						to="/managereferences"
						>Manage References</RouterLink
					>

					<RouterLink
						v-if="userHasRole(Roles.EDITTOPICS)"
						class="nav-item nav-button"
						to="/managetopics"
						>Manage Topics</RouterLink
					>

					<RouterLink
						v-if="userHasRole(Roles.EDITSOURCES)"
						class="nav-item nav-button"
						to="/managesources"
						>Manage Sources</RouterLink
					>
					<!-- <a-button class="nav-item">MyPages</a-button> -->
					<a-button
						class="nav-item"
						@click="handleLogout"
						>Logout</a-button
					>
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
	background: var(--color-backgroundNavAndFooter);
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
}

.nav-button {
	color: var(--color-textNavLink);
	font-size: larger;
}

.discrete {
	opacity: 0.9;
}

.navigation-right {
	display: flex;
	align-items: center;
}
</style>
