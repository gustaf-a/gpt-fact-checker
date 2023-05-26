<script setup lang="ts">
import Container from "../Container.vue";
</script>

<template>
	<a-layout-header class="hero-banner">
		<Container>
			<div class="hero-container">
				<h1 class="hero-title">FactCheckMe</h1>
				<p class="hero-subtext">
					Clearly presented claims - easy to check
				</p>
			</div>
		</Container>
	</a-layout-header>
</template>
<style scoped>
.hero-banner {
	background-color: #9f9f9f;
	height: 8vh;
}

h1{
	margin:0;
	padding:0;
}

.hero-container {
	display: flex;
	color: #ffffff;
	justify-content: center;
}

.hero-title {
	color: #ffffff;
	opacity: 0.7;
	font-size: 2em;
	font-weight: bold;
}

.hero-subtext {
	margin-left: 2vw;
	font-size: 1.2em;
	opacity: 0.7;
}
</style>
