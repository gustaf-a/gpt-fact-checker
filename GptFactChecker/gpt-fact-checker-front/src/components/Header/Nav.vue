<script setup lang="ts">
import Container from "../Container.vue";
import { ref} from "vue"

const isLoggedIn = ref(true);

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

					<RouterLink
						class="nav-item nav-button"
						to="/resources"
						>Resources</RouterLink
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
				<div class="navigation-right">
					<div v-if="isLoggedIn">
						<RouterLink
						class="nav-item nav-button"
						to="/managesources"
						>Manage Sources</RouterLink
					>
						<!-- <a-button class="nav-item">MyPages</a-button> -->
						<a-button class="nav-item">Logout</a-button>
					</div>
					<div v-else>
						<a-button class="nav-item">Sign up</a-button>
						<a-button class="nav-item">Login</a-button>
					</div>
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

.navigation-right {
	display: flex;
	align-items: center;
}
</style>
